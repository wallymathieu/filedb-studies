
using ProtoBuf;

namespace SomeBasicFileStoreApp.Core.Commands
{
	[ProtoContract]
	public class AddOrderCommand : Command
	{
		[ProtoMember(1)]
		public Order Object { get; set; }
	}
}
