using ChessCake.Commons.Enumerations;
using ChessCake.Models.Pieces.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Pieces {
    class Queen : BasePiece {

        public const PieceType pieceType = PieceType.QUEEN;
        public Queen(ChessColor color) : base(color) {
            Type = PieceType.QUEEN;
        }

        public override string ToString() {
            return "Q";
        }

    }
}
