using System;
using System.IO;
using System.Threading.Tasks;
using AServiceBus.Core;
using AServiceBus.Core.Receiver;
using Microsoft.ServiceBus.Messaging;

namespace AServiceBus.Transport.AzureServiceBus
{
    internal class BrokeredMessageProcessor
    {
        private readonly BrokeredMessageFactory _brokeredMessageFactory;
        private readonly HandlerFactory _handlerFactory;

        public BrokeredMessageProcessor(BrokeredMessageFactory brokeredMessageFactory, HandlerFactory handlerFactory)
        {
            _handlerFactory = handlerFactory;
            _brokeredMessageFactory = brokeredMessageFactory;
        }

        internal async Task Process(BrokeredMessage brokeredMessage)
        {
            try
            {
                var stream = brokeredMessage.GetBody<Stream>();
                var message = _brokeredMessageFactory.From(stream);

                var handlerResult = await _handlerFactory.Handle(message);

                var attemptedTimestamp = DateTimeProvider.Instance.Now;
                var publishedAt = new DateTimeOffset(brokeredMessage.EnqueuedTimeUtc);
                var durationSincePublishMs = DateTimeProvider.Instance.Now.ToUnixTimeMilliseconds() - publishedAt.ToUnixTimeMilliseconds();

                switch (handlerResult.Status)
                {
                    case HandlerResult.StatusCodes.HandledSuccessfully:
                        await brokeredMessage.CompleteAsync();
                        Logging.Logger
                            .ForContext(message.GetType())
                            .ForContext("MessageId", brokeredMessage.MessageId)
                            .ForContext("CorrelationId", brokeredMessage.CorrelationId)
                            .ForContext("Attempt", brokeredMessage.DeliveryCount)
                            .ForContext("PublishedAt", publishedAt)
                            .ForContext("AttemptedAt", attemptedTimestamp)
                            .ForContext("HandleDurationMs", handlerResult.DurationMs)
                            .ForContext("DurationSincePublishMs", durationSincePublishMs)
                            //.ForContext("FromQueue", _queue.QueueName)
                            .Information("Message handled.");
                        break;

                    case HandlerResult.StatusCodes.MessageHandlerNotFound:
                        await brokeredMessage.AbandonAsync();
                        Logging.Logger
                            .ForContext(message.GetType())
                            .ForContext("MessageId", brokeredMessage.MessageId)
                            .ForContext("CorrelationId", brokeredMessage.CorrelationId)
                            .ForContext("Attempt", brokeredMessage.DeliveryCount)
                            .ForContext("PublishedAt", publishedAt)
                            .ForContext("AttemptedAt", attemptedTimestamp)
                            .ForContext("DurationSincePublishMs", durationSincePublishMs)
                            //.ForContext("FromQueue", _queue.QueueName)
                            .Error("Message handler not found for message type.");
                        break;

                    case HandlerResult.StatusCodes.UnhandledErrorDuringHandlerExecution:
                        await brokeredMessage.AbandonAsync();
                        Logging.Logger
                            .ForContext(message.GetType())
                            .ForContext("MessageId", brokeredMessage.MessageId)
                            .ForContext("CorrelationId", brokeredMessage.CorrelationId)
                            .ForContext("Attempt", brokeredMessage.DeliveryCount)
                            .ForContext("PublishedAt", publishedAt)
                            .ForContext("AttemptedAt", attemptedTimestamp)
                            .ForContext("DurationSincePublishMs", durationSincePublishMs)
                            //.ForContext("FromQueue", _queue.QueueName)
                            .Error(handlerResult.Exception, "Unhandled error occured while executing message handler.");
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception exception)
            {
                await brokeredMessage.AbandonAsync();

                var stream = brokeredMessage.GetBody<Stream>();
                var body = new StreamReader(stream, true).ReadToEnd();

                Logging.Logger
                    .ForContext("MessageId", brokeredMessage.MessageId)
                    .ForContext("CorrelationId", brokeredMessage.CorrelationId)
                    .ForContext("Body", body)
                    //.ForContext("FromQueue", _queue.QueueName)
                    .Error(exception, "Unhandled error occured consuming message.");
            }
        }
    }
}
