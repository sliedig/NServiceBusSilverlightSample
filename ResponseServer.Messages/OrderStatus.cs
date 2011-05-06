using System;
using System.Runtime.Serialization;

namespace ResponseServer.Messages
{

	public enum OrderStatus
	{
		Cancelled,
		PendingCancellation,
		CancellationFailed
	}
}
