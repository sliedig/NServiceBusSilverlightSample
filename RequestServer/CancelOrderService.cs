using System;
using RequestServer.Messages;
using NServiceBus;

namespace RequestServer
{
	/// <summary>
	/// NServiceBus WCF implementation.
	/// </summary>
	public class CancelOrderService : WcfService<CancelOrderRequest, ErrorCodes>
	{
	}
}
