using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcutController
{
    [Serializable]
    public class CutNotOpenException : Exception
    {
        public CutNotOpenException() { }
        public CutNotOpenException(string message) : base(message) { }
        public CutNotOpenException(string message, Exception inner) : base(message, inner) { }
        protected CutNotOpenException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class CutNotExistException : Exception
    {
        public CutNotExistException() { }
        public CutNotExistException(string message) : base(message) { }
        public CutNotExistException(string message, Exception inner) : base(message, inner) { }
        protected CutNotExistException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class CutInvokeIncorrectException : Exception
    {
        public CutInvokeIncorrectException() { }
        public CutInvokeIncorrectException(string message) : base(message) { }
        public CutInvokeIncorrectException(string message, Exception inner) : base(message, inner) { }
        protected CutInvokeIncorrectException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
