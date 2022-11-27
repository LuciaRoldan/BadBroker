using System.Runtime.Serialization;

namespace BadBroker.Api.Exceptions
{
    [Serializable]
    public class NoRevenueException : Exception
    {
        public NoRevenueException()
        {
        }

        public NoRevenueException(string? message) : base(message)
        {
        }

        public NoRevenueException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NoRevenueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}