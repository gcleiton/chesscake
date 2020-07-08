using ChessCake.Commons.Enumerations;
using ChessCake.Models.Pieces.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Players.Contracts {
    interface IPlayer {
        public string Name { get; }
        public ChessColor Color { get; }
        public IList<BasePiece> Pieces { get; }

        public void addPiece(BasePiece piece);

        public void removePiece(BasePiece piece);

        public bool isPieceExists(BasePiece piece);
    }
}
