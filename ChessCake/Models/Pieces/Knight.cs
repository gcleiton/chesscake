using ChessCake.Commons.Enumerations;
using ChessCake.Models.Pieces.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Pieces {
    class Knight : BasePiece {

        public const PieceType pieceType = PieceType.KNIGHT;
        public Knight(ChessColor color) : base(color) {
            Type = PieceType.KNIGHT;
        }

        public override string ToString() {
            return "N";
        }

    }
}
