using ChessCake.Commons.Enumerations;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Positions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Pieces {
    class Empty : BasePiece {
        public Empty(ChessColor color, IPosition position) : base(color, position) {
            Type = PieceType.EMPTY;
        }

        public override string ToString() {
            return "E";
        }
    }
}
