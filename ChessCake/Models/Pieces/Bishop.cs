using ChessCake.Commons.Enumerations;
using ChessCake.Models.Boards;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Boards.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Positions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Pieces {
    class Bishop : BasePiece {

        public Bishop(ChessColor color, IPosition position) : base(color, position) {
            Type = PieceType.BISHOP;
        }
    
        public override string ToString() {
            return "B";
        }

    }
}
