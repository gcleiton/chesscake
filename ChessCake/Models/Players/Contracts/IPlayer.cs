using ChessCake.Commons.Enumerations;
using ChessCake.Models.Pieces.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Players.Contracts {
    public interface IPlayer {
        public string Name { get; }
        public ChessColor Color { get; }
        public IList<BasePiece> Pieces { get; }

        public void AddPiece(BasePiece piece);

        public void removePiece(BasePiece piece);

        public bool isPieceExists(BasePiece piece);
    }
}
