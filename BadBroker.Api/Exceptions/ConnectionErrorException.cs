using System.Runtime.Serialization;

namespace BadBroker.Api.Exceptions
{
    [Serializable]
    public class ConnectionErrorException : Exception
    {
        public ConnectionErrorException()
        {
        }

        public ConnectionErrorException(string? message) : base(message)
        {
        }

        public ConnectionErrorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ConnectionErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}