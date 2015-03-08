namespace SomeBasicFileStoreApp.Core.Commands
{
	public class AddProductCommandHandler : ICommandHandler<AddProductCommand>
	{
		private readonly IRepository _repository;
		public AddProductCommandHandler(IRepository repository)
		{
			_repository = repository;
		}

		public void Handle(AddProductCommand command)
		{
			_repository.Save(new Product(command.Id, command.Cost, command.Name, command.Version));
		}
	}
}
