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
			_repository.Save(command.Object);
		}
	}
}
