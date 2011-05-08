using System;
using NServiceBus;
using ResponseServer.Messages;
using RequestServer.Messages;

namespace RequestServer
{
	public class CancelOrderHandler : IHandleMessages<CancelOrderRequest>
	{
		private readonly IBus bus;

		public CancelOrderHandler(IBus bus)
		{
			this.bus = bus;
		}
		
		/// <summary>
		/// Handles Cancel Order requests from Silverlight application.
		/// </summary>
		/// <param name="message">CancelOrderRequest</param>
		public void Handle(CancelOrderRequest message)
		{
			try
			{
				// TODO: Implement Saga so we can simulate the multiple response scenario.
				bus.Send<OrderResponse>(response =>
					{
						response.OrderId = message.OrderId;
						response.ConfirmationId = Guid.NewGuid();
						response.Status = OrderStatus.PendingCancellation;
					}
				);


				// Return error code indicating successful processing of request.
				bus.Return((int)ErrorCodes.Success);
			}
			catch (Exception ex)
			{
				bus.Return((int)ErrorCodes.Fail);
			}

		}
	}
}