using System;
using System.ServiceModel;

namespace ResponseServer
{
	[ServiceContract]
	public interface IOrderPollingClient
	{
		[OperationContract(IsOneWay = true)]
		void ReceiveOrderCancellations(string sessionId);

		[OperationContract(IsOneWay = true, AsyncPattern = true)]
		IAsyncResult BeginCancelOrderResponse(CancelOrderResponseData pollingResponse, AsyncCallback callback, object state);

		void EndCancelOrderResponse(IAsyncResult result);

	}
}
