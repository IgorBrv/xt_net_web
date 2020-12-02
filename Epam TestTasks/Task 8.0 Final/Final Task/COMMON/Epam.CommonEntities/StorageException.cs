using System.Runtime.Serialization;
using System;

namespace Epam.CommonEntities
{
    public class StorageException : Exception
    {   // Самописный класс исключения для DAL

        public StorageException() { }

        public StorageException(string message) : base(message) { }

        public StorageException(string message, Exception inner) : base(message, inner) { }

        protected StorageException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
