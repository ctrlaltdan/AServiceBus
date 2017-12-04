using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AServiceBus.Contracts;

namespace AServiceBus.Core.Receiver
{
    public class HandlerFactory
    {
        private readonly IResolver _resolver;

        public HandlerFactory(IResolver resolver)
        {
            _resolver = resolver;
        }

        public async Task<HandlerResult> Handle<TMessage>(TMessage message)
            where TMessage : IMessage
        {
            var stopwatch = Stopwatch.StartNew();
        
            var handler = _resolver.GetInstance<IHandle<TMessage>>(message, typeof(IHandle<>));
            if (handler == null)
                return HandlerResult.ForHandlerNotFound(stopwatch.ElapsedMilliseconds);
            
            try
            {
                await handler.HandleAsync(message);
                return HandlerResult.ForSuccess(stopwatch.ElapsedMilliseconds);
            }
            catch (Exception exception)
            {
                return HandlerResult.ForUnhandledError(exception, stopwatch.ElapsedMilliseconds);
            }
        }
    }
}