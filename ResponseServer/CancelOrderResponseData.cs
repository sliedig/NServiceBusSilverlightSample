using System;
using System.Runtime.Serialization;
using ResponseServer.Messages;

namespace ResponseServer
{

	[DataContract]
	public class CancelOrderResponseData
	{
		[DataMember]
		public int OrderId { get; set; }
		[DataMember]
		public OrderStatus Status { get; set; }
		[DataMember]
		public Guid ConfirmationId { get; set; }
	}
}
