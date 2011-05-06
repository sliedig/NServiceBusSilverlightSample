using System;
using System.ServiceModel;

namespace ResponseServer
{
		
	[ServiceContract(CallbackContract = typeof(IOrderPollingClient))]
	public interface IOrderPollingService
	{
		[OperationContract(IsOneWay = true)]
		void GetOrderCancellations();
	}

}
