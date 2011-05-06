using System;
using NServiceBus;

namespace RequestServer.Messages
{
	public class CancelOrderRequest : IMessage
	{
		public int OrderId { get; set; }
	}
}
