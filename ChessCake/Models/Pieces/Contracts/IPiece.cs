using ChessCake.Commons.Enumerations;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Positions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Pieces.Contracts {
    public interface IPiece {
        public ChessColor Color { get; }

        public IPosition Position { get; }

        public PieceType Type { get; }

        public int MoveCount { get; }

    }
}
