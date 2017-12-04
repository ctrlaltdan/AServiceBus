using System;

namespace AServiceBus.Core
{
    public interface IResolver
    {
        T GetInstance<T>();
        T GetInstance<T>(object message, Type handlerInterface);
    }
}
