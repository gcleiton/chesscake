using System;
using System.Runtime.Serialization;

namespace ChessCake.Engines {
    [Serializable]
    internal class IllegalStateException : Exception {
        private object p;

        public IllegalStateException() {
        }

        public IllegalStateException(object p) {
            this.p = p;
        }

        public IllegalStateException(string message) : base(message) {
        }

        public IllegalStateException(string message, Exception innerException) : base(message, innerException) {
        }

        protected IllegalStateException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}