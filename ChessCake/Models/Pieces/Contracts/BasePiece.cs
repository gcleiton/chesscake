using ChessCake.Commons.Enumerations;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Boards.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Pieces.Contracts {
    public abstract class BasePiece : IPiece {

        public ChessColor Color { get; private set; }

        public PieceType Type { get; protected set; }

        public bool IsAvailable { get; set; }

        public int MoveCount { get; set; }

        public BasePiece(ChessColor color) {
            Color = color;
            MoveCount = 0;
        }
        public void increaseMoveCount() {
            MoveCount++;
        }

    }
}
