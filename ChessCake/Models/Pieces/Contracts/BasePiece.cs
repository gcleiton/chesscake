using ChessCake.Commons.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Pieces.Contracts {
    public abstract class BasePiece : IPiece {
        public ChessColor Color { get; private set; }

        public bool IsAvailable { get; set; }

        public BasePiece(ChessColor color) {
            this.Color = color;        
        }
    }
}
