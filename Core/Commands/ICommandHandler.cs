using System;
using System.Linq;

namespace SomeBasicFileStoreApp.Core
{
	public interface ICommandHandler<T> : ICommandHandler
	{
		void Handle(T command);
	}
	public interface ICommandHandler
	{
	}

	public static class ICommandHandlerExtensions
	{
		public static void Handle(this ICommandHandler self, dynamic command)
		{
			dynamic handler = self;
			handler.Handle(command);
		}

		public static bool CanHandle<T>(this ICommandHandler self)
		{
			var handler = self as ICommandHandler<T>;
			return handler != null;
		}
		public static bool CanHandle(this ICommandHandler self, object command)
		{
			var chandler = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
			return chandler.IsInstanceOfType(self);
		}
		public static Type[] GetCommandTypes(this ICommandHandler self)
		{
			var g = typeof(ICommandHandler<>);
			var interfaces = self.GetType().GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == g);
			return interfaces
				.Select(i => i.GetGenericArguments().Single())
				.ToArray();
		}

	}
}