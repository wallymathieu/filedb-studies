using ProtoBuf;

namespace SomeBasicFileStoreApp.Core
{
	[ProtoContract]
	[ProtoInclude(1, typeof(AddCustomerCommand))]
	[ProtoInclude(2, typeof(AddProductCommand))]
	[ProtoInclude(3, typeof(AddOrderCommand))]
	public class Command
	{
	}
}