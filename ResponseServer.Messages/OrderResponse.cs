using System;
using NServiceBus;

namespace ResponseServer.Messages
{
	public class OrderResponse : IMessage
	{
		public int OrderId { get; set; }
		public Guid ConfirmationId { get; set; }
		public OrderStatus Status { get; set; }
	}
}
