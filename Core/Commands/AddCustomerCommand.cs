using ProtoBuf;

namespace SomeBasicFileStoreApp.Core.Commands
{
	[ProtoContract]
	public class AddCustomerCommand : Command
	{
		[ProtoMember(1)]
		public Customer Object { get; set; }
	}
}
