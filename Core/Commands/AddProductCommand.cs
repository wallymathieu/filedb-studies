using ProtoBuf;

namespace SomeBasicFileStoreApp.Core.Commands
{
	[ProtoContract]
	public class AddProductCommand : Command
	{
		[ProtoMember(1)]
		public Product Object { get; set; }
	}
}
