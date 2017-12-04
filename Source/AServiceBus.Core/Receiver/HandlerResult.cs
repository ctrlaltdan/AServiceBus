using System;

namespace AServiceBus.Core.Receiver
{
    public class HandlerResult
    {
        public enum StatusCodes
        {
            HandledSuccessfully = 0,
            MessageHandlerNotFound = 1,
            UnhandledErrorDuringHandlerExecution = 2
        }

        public StatusCodes Status;

        public bool IsSuccessful => Status == StatusCodes.HandledSuccessfully;
        public Exception Exception;
        public long DurationMs;

        public static HandlerResult ForSuccess(long ellapsedMilliseconds)
        {
            return new HandlerResult
            {
                Status = StatusCodes.HandledSuccessfully,
                DurationMs = ellapsedMilliseconds
            };
        }

        public static HandlerResult ForHandlerNotFound(long ellapsedMilliseconds)
        {
            return new HandlerResult
            {
                Status = StatusCodes.MessageHandlerNotFound,
                DurationMs = ellapsedMilliseconds,
            };
        }

        public static HandlerResult ForUnhandledError(Exception exception, long ellapsedMilliseconds)
        {
            return new HandlerResult
            {
                Status = StatusCodes.UnhandledErrorDuringHandlerExecution,
                DurationMs = ellapsedMilliseconds,
                Exception = exception
            };
        }
    }
}