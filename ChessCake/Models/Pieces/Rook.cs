using ChessCake.Commons.Enumerations;
using ChessCake.Models.Pieces.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Pieces {
    class Rook : BasePiece {

        public const PieceType pieceType = PieceType.ROOK;
        public Rook(ChessColor color) : base(color) {
            Type = PieceType.ROOK;
        }

        public override string ToString() {
            return "R";
        }

    }
}
