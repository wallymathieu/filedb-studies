using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SomeBasicFileStoreApp.Core
{
	public class AddCustomerCommandHandler : ICommandHandler<AddCustomerCommand>
	{
		private readonly IRepository _repository;
		public AddCustomerCommandHandler(IRepository repository)
		{
			_repository = repository;
		}

		public void Handle(AddCustomerCommand command)
		{
			_repository.Save(command.Object);
		}
	}
	public class AddProductCommandHandler : ICommandHandler<AddProductCommand>
	{
		private readonly IRepository _repository;
		public AddProductCommandHandler(IRepository repository)
		{
			_repository = repository;
		}

		public void Handle(AddProductCommand command)
		{
			_repository.Save(command.Object);
		}
	}
	public class AddOrderCommandHandler : ICommandHandler<AddOrderCommand>
	{
		private readonly IRepository _repository;
		public AddOrderCommandHandler(IRepository repository)
		{
			_repository = repository;
		}

		public void Handle(AddOrderCommand command)
		{
			_repository.Save(command.Object);
		}
	}
	[ProtoContract]
	public class AddCustomerCommand : Command	{
		[ProtoMember(1)]
		public Customer Object { get; set; }
	}
	[ProtoContract]
	public class AddProductCommand : Command
	{
		[ProtoMember(1)]
		public Product Object { get; set; }
	}
	[ProtoContract]
	public class AddOrderCommand : Command
	{
		[ProtoMember(1)]
		public Order Object { get; set; }
	}

	public class Model
	{
		static Model()
		{
			var model = RuntimeTypeModel.Default;
			model[typeof(Customer)]
				.Add(1, "Id")
				.Add(2, "Firstname")
				.Add(3, "Lastname")
				.Add(4, "Orders")
				.Add(5, "Version")
				;

			model[typeof(Product)]
				.Add(1, "Id")
				.Add(2, "Cost")
				.Add(3, "Name")
				.Add(4, "Version")
				;

			model[typeof(Order)]
				.Add(1, "Id")
				.Add(2, "Customer")
				.Add(3, "OrderDate")
				.Add(4, "Products")
				.Add(5, "Version")
				;
		}

	}
}
