namespace AServiceBus.Core.Receiver
{
    public enum QueueProcessingResult
    {
        ErrorProcessingQueue,
        QueueProcessedSuccessfully,
        NoMessagesFoundInQueue
    }
}