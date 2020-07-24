using ChessCake.Commons;
using ChessCake.Models.Positions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.ChessPositions.Contracts {
    interface IChessPosition {
        public char Column { get; }
        public int Row { get; }
        public Position ToPosition();

    }
}
