using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeBasicFileStoreApp.Core.Commands
{
	[ProtoContract]
	public class AddProductToOrder :Command
	{
		[ProtoMember(1)]
		public int OrderId { get; set; }
		[ProtoMember(2)]
		public int ProductId { get; set; }

	}
}
