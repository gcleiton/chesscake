using ChessCake.Commons.Enumerations;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Boards.Contracts;
using ChessCake.Models.ChessPositions.Contracts;
using ChessCake.Models.Positions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Pieces.Contracts {
    public abstract class BasePiece : IPiece {

        public ChessColor Color { get; private set; }

        public IPosition Position { get; set; }

        public PieceType Type { get; protected set; }

        public bool IsAvailable { get; set; }

        public int MoveCount { get; set; }

        public BasePiece(ChessColor color, IPosition position) {
            Color = color;
            Position = position;
            MoveCount = 0;
        }
        public void IncreaseMoveCount() {
            MoveCount++;
        }

        public void DecreaseMoveCount() {
            MoveCount--;
        }

    }
}
