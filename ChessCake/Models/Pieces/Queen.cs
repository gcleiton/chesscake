using ChessCake.Commons.Enumerations;
using ChessCake.Models.Pieces.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Pieces {
    class Queen : BasePiece {
        public Queen(ChessColor color) : base(color) {

        }

        public override string ToString() {
            return "Q";
        }

    }
}
