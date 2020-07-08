using System;

namespace ChessCake.Exceptions {
    class BoardException : Exception {
        public BoardException(string msg) : base(msg) { }
    }
}
