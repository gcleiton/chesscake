using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Players.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Movements.Contracts {
    interface IMovement {

        public IPlayer player { get; }
        public ICell Source { get; }
        public ICell Target { get; }
        public IPiece MovedPiece { get; }
        public IPiece CapturedPiece { get; }

    }
}
