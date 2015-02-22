﻿namespace SomeBasicFileStoreApp.Core.Commands
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
}