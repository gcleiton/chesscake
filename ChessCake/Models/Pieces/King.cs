using ChessCake.Commons.Enumerations;
using ChessCake.Models.Pieces.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Pieces {
    class King : BasePiece {

        public King(ChessColor color) : base(color) {
            Type = PieceType.KING;
        }

        public override string ToString() {
            return "K";
        }
    }
}
