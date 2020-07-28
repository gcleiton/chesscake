using ChessCake.Commons;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Movements.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Players.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Movements {
    class Movement : IMovement {
        public IPlayer player { get; set; }
        public ICell Source { get; set; }
        public ICell Target { get; set; }
        public IPiece MovedPiece { get; set; }
        public IPiece CapturedPiece { get; set; }

        public Movement(ICell source, ICell target) {
            //this.player = player;
            this.Source = source;
            this.Target = target;
            this.MovedPiece = source.Piece;

            if(!Common.IsObjectNull(Target.Piece)) {
                this.CapturedPiece = Target.Piece;
            }
        }

    }
}
