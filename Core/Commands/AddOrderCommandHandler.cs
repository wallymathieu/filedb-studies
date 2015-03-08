namespace SomeBasicFileStoreApp.Core.Commands
{
	public class AddOrderCommandHandler : ICommandHandler<AddOrderCommand>
	{
		private readonly IRepository _repository;
		public AddOrderCommandHandler(IRepository repository)
		{
			_repository = repository;
		}

		public void Handle(AddOrderCommand command)
		{
			_repository.Save(new Order(command.Id, command.Customer, command.OrderDate, new Product[0], command.Version));
		}
	}
}
