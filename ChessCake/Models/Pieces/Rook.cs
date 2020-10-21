using ChessCake.Commons.Enumerations;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Positions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Pieces {
    class Rook : BasePiece {
        public Rook(ChessColor color, IPosition position) : base(color, position) {
            Type = PieceType.ROOK;
        }

        public override string ToString() {
            return "R";
        }

    }
}
