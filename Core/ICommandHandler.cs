using System;
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
        public static void Handle(this ICommandHandler self,dynamic command)
        {
			dynamic handler = self;
			handler.Handle(command);
        }

       public static bool CanHandle<T>(this ICommandHandler self)
       {
            var handler = self as ICommandHandler<T>;
            return handler!=null;
        }
       public static bool CanHandle(this ICommandHandler self, object command)
       {
            var chandler = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            return chandler.IsInstanceOfType(self);
        }
    }
}