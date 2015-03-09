using System;
using System.Linq;
using System.Collections.Generic;

namespace SomeBasicFileStoreApp.Core
{
	public interface ICommandHandler<T> : ICommandHandler
	{
		void Handle(T command);
	}
	public interface ICommandHandler
	{
	}

}