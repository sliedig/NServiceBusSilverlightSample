using System;
using NServiceBus;

namespace ResponseServer.Messages
{
	public interface OrderResponse : IMessage
	{
		int OrderId { get; set; }
		Guid ConfirmationId { get; set; }
		OrderStatus Status { get; set; }
	}
}
