using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using With;
namespace SomeBasicFileStoreApp.Core.Commands
{
	[ProtoContract]
	public class AddProductToOrder :Command
	{
		[ProtoMember(1)]
		public int OrderId { get; set; }
		[ProtoMember(2)]
		public int ProductId { get; set; }

        public override void Handle(IRepository _repository)
        {
            var command = this;
            var order = _repository.GetOrder(command.OrderId);
            var product = _repository.GetProduct(command.ProductId);
            var products = new List<Product>(order.Products);
            products.Add(product);
            _repository.Save(order.With().Eql(o=>o.Products, products));
        }
	}
}
