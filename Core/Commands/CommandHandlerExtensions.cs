using System;
using System.Linq;
using System.Collections.Generic;

namespace SomeBasicFileStoreApp.Core
{
	public static class CommandHandlerExtensions
	{
		public static void Handle(this ICommandHandler self, dynamic command)
		{
			dynamic handler = self;
			handler.Handle(command);
		}

		public static bool CanHandle(this ICommandHandler self, Type command)
		{
            var expectedCommandTypes = GetCommandTypes(self);
            return expectedCommandTypes.Any(t => t.IsAssignableFrom(command));
		}

		public static Type[] GetCommandTypes(this ICommandHandler self)
		{
            // we cache this operation, since it's a static property of ICommandHandler type
            return Cache(self.GetType(), t =>
                {
                    var g = typeof(ICommandHandler<>);
                    var interfaces = self.GetType().GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == g);
                    return interfaces
                        .Select(i => i.GetGenericArguments().Single())
                        .ToArray();
                });
		}

        private static IDictionary<Type,Type[]> cache = new Dictionary<Type,Type[]>();
        private static Type[] Cache(Type val, Func<Type,Type[]> map){
            if (!cache.ContainsKey(val))
            {
                cache[val] = map(val);
            }
            return cache[val];
        }
	}
}