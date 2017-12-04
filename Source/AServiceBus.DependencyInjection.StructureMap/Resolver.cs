using System;
using AServiceBus.Core;
using StructureMap;

namespace AServiceBus.DependencyInjection.StructureMap
{
    public class Resolver : IResolver
    {
        private readonly IContainer _container;
        
        internal Resolver(IContainer container)
        {
            _container = container;
        }

        public T GetInstance<T>()
        {
            using (var scope = _container.GetNestedContainer())
            {
                return scope.TryGetInstance<T>();
            }
        }

        public T GetInstance<T>(object message, Type handlerInterface)
        {
            using (var scope = _container.GetNestedContainer())
            {
                return scope.ForObject(message)
                    .GetClosedTypeOf(handlerInterface)
                    .As<T>();
            }
        }
    }
}
