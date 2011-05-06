using System;
using System.ServiceModel;

namespace RequestServer.Messages
{
	[ServiceContract]
	public interface ICancelOrderService
	{
		[OperationContract(Action = "http://tempuri.org/IWcfServiceOf_CancelOrder_ErrorCodes/Process", ReplyAction = "http://tempuri.org/IWcfServiceOf_CancelOrder_ErrorCodes/ProcessResponse")]
		ErrorCodes Process(CancelOrderRequest request);

	}
}
