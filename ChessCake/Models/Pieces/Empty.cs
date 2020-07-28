using ChessCake.Commons.Enumerations;
using ChessCake.Models.Pieces.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Pieces {
    class Empty : BasePiece {
        public Empty(ChessColor color) : base(color) {
            Type = PieceType.EMPTY;
        }

        public override string ToString() {
            return "E";
        }
    }
}
