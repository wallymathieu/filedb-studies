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
			_repository.Save(command.Object);
		}
	}
}
