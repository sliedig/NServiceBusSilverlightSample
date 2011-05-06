using System;
using RequestServer.Messages;
using NServiceBus;

namespace RequestServer
{
	public class CancelOrderService : WcfService<CancelOrderRequest, ErrorCodes>
	{
	}
}
