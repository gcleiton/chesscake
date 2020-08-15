using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Players.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Movements.Contracts {
    public interface IMovement {

        public IPlayer Player { get; }
        public ICell Source { get; }
        public ICell Target { get; }
        public BasePiece MovedPiece { get; set; }
        public BasePiece CapturedPiece { get; set;  }

    }
}
