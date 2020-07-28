using ChessCake.Commons.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Pieces.Contracts {
    public interface IPiece {
        public ChessColor Color { get; }

        public PieceType Type { get; }

    }
}
