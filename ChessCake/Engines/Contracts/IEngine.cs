using ChessCake.Commons.Enumerations;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Boards.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Players.Contracts;
using ChessCake.Models.Positions.Contracts;
using ChessCake.Providers.Movements.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Engines.Contracts {
    public interface IEngine {
        public IBoard Board { get; }

        public IPlayer CurrentPlayer { get; set; }

        public int Turn { get; }

        public GameStatus Status { get; }

        public BasePiece EnPassant { get; }

        public BasePiece Promoted { get; }

        public IDictionary<IPlayer, IList<BasePiece>> Pieces { get; }

        public IDictionary<IPlayer, IList<BasePiece>> CapturedPieces { get; }

        public IDictionary<ChessColor, IPlayer> Players { get; }

        public void Initialize();

    }
}
