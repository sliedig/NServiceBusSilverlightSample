﻿using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using ResponseServer.Messages;
using NServiceBus;

namespace ResponseServer
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
	public class OrderPollingService : IOrderPollingService, IHandleMessages<OrderResponse>
	{

		private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		readonly object _key = new object();
		private static Dictionary<string, IOrderPollingClient> _clientCallbackStore = new Dictionary<string, IOrderPollingClient>();
		readonly static AsyncCallback _receiveOrderCancelResponseCompleted = new AsyncCallback(ReceiveOrderCancelResponseCompleted);


		#region IOrderPollingService Members

		public void GetOrderCancellations()
		{
			//Get client callback channel
			var context = OperationContext.Current;
			var sessionID = context.SessionId;
			var currClient = context.GetCallbackChannel<IOrderPollingClient>();
			context.Channel.Faulted += DisconnectClient;
			context.Channel.Closed += DisconnectClient;

			IOrderPollingClient client;
			if (!_clientCallbackStore.TryGetValue(sessionID, out client))
			{
				lock (_key)
				{
					_log.Info(string.Format("Adding callback channel. Session Id: {0}", sessionID));
					_clientCallbackStore[sessionID] = currClient;
				}
			}

			// Return the Session Id to the client. Provides a visual indicator that registration has haken place. 
			currClient.ReceiveOrderCancellations(sessionID);

		}

		#endregion

		/// <summary>
		/// Loops through stored sessions, and pushes responses to open channels.
		/// </summary>
		/// <param name="response">CancelOrderResponse DTO</param>
		public static void PushResponse(CancelOrderResponseData response)
		{
			IEnumerable<KeyValuePair<string, IOrderPollingClient>> callbackChannels
				= _clientCallbackStore.Where(cb => ((IContextChannel)cb.Value).State == CommunicationState.Opened);


			// TODO: Filter so that the response message is sent to the originating client only, rather than a broadcast.

			for (int i = 0; i < callbackChannels.Count(); i++)
			{
				IOrderPollingClient cb = callbackChannels.ElementAt(i).Value;

				try
				{
					cb.BeginCancelOrderResponse(response, _receiveOrderCancelResponseCompleted, cb);
				}
				catch (TimeoutException ex)
				{
					_log.Error(ex.Message, ex);
				}
				catch (CommunicationException ex)
				{
					_log.Error(ex.Message, ex);
				}
			}

		}

		private static void ReceiveOrderCancelResponseCompleted(IAsyncResult ar)
		{
			try
			{
				((IOrderPollingClient)(ar.AsyncState)).EndCancelOrderResponse(ar);
			}
			catch (CommunicationException ex)
			{
				_log.Error(ex.Message, ex);
			}
			catch (TimeoutException ex)
			{
				_log.Error(ex.Message, ex);
			}
		}

		private void DisconnectClient(object sender, EventArgs e)
		{
			lock (_key)
			{
				try
				{
					var sessionID = OperationContext.Current.SessionId;
					_clientCallbackStore.Remove(sessionID);
				}
				catch (CommunicationException ex)
				{
					_log.Error(ex.Message, ex);
				}
			}
		}




		#region IMessageHandler<OrderResponse> Members
		/// <summary>
		/// Processes the Order responses from Saga (note: Saga not yet implemented).
		/// </summary>
		/// <param name="message"></param>
		public void Handle(OrderResponse message)
		{
			var response = new CancelOrderResponseData() { ConfirmationId = message.ConfirmationId, Status = message.Status, OrderId = message.OrderId };
			OrderPollingService.PushResponse(response);
		}

		#endregion
	}
}
