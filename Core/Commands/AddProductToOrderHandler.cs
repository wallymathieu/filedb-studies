using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeBasicFileStoreApp.Core.Commands
{
	public class AddProductToOrderHandler : ICommandHandler<AddProductToOrder>
	{
		private readonly IRepository _repository;
		public AddProductToOrderHandler(IRepository repository)
		{
			_repository = repository;
		}

		public void Handle(AddProductToOrder command)
		{
			var order = _repository.GetOrder(command.OrderId);
			var product = _repository.GetProduct(command.ProductId);
			order.Products.Add(product);
            _repository.Save(order);
		}
	}
}
