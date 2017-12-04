using System;

namespace AServiceBus.Core.Domain
{
    public class QueueIdentifier
    {
        public string QueueName { get; }

        public static QueueIdentifier For(string queueName)
        {
            return new QueueIdentifier(queueName);
        }
        
        private QueueIdentifier(string queueName)
        {
            if (string.IsNullOrWhiteSpace(queueName))
                throw new ArgumentOutOfRangeException(nameof(queueName), queueName, "Cannot be null or empty.");
            
            QueueName = queueName;
        }
        
        public override string ToString()
        {
            return $"{QueueName}";
        }

        protected bool Equals(QueueIdentifier other)
        {
            return string.Equals(QueueName, other.QueueName, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((QueueIdentifier)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return QueueName?.GetHashCode() ?? 0;
            }
        }
    }
}
