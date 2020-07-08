using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Exceptions {
    class ChessException : Exception {
        public ChessException(string msg) : base(msg) { }

    }
}
