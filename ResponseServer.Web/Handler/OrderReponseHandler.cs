using System;
using ResponseServer.Messages;

namespace ResponseServer.Web
{
	public class OrderReponseHandler : NServiceBus.IHandleMessages<OrderResponse>
	{

		#region IMessageHandler<OrderResponse> Members

		public void Handle(OrderResponse message)
		{
			if (message == null)
				return;

			
			var service = new OrderPollingService();
			var response = new CancelOrderResponseData() { ConfirmationId = message.ConfirmationId, Status = message.Status };
			service.PushResponse(response);

			//var singleton = ((ServiceHost)OperationContext.Current.Host).SingletonInstance;
			//Type singletonType = singleton.GetType();


			//	CancelOrderResponseData responseData = new CancelOrderResponseData();
			//	responseData.ConfirmationId = message.ConfirmationId;
			//	responseData.Status = message.Status;

			//
			//MethodInfo method = typeof(singleton.GetType).GetMethod("PushResponse");
			//FieldInfo field = typeof(ResponseServer.OrderPollingService).GetField("instance", BindingFlags.Static | BindingFlags.Public);
			//object instance = field.GetValue(null);
			//method.Invoke(instance, responseData);


		}

		#endregion
	}
}