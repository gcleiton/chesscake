using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Exceptions {
    class ChessException : Exception {
        private string ErrorMessage;

        public ChessException(string msg) : base(msg) {
            this.ErrorMessage = msg;
        }

        public override string Message {
            get {
                return "\n| CHESSCAKE CONSOLE | " + ErrorMessage;
            }
        }

    }
}
