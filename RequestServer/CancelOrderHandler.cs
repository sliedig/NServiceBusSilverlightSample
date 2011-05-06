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

		public void Handle(CancelOrderRequest message)
		{
			// logic here...

			bus.Return((int)ErrorCodes.Success);

			// Probably best to publish message to Saga so we can simulate the multiple response scenario.
			bus.Send<OrderResponse>(r =>
			{
				r.OrderId = message.OrderId;
				r.ConfirmationId = Guid.NewGuid(); 
				r.Status = OrderStatus.PendingCancellation;
			}
			);

		}
	}
}