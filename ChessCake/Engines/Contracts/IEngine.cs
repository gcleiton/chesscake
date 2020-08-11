﻿using ChessCake.Commons.Enumerations;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Boards.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Players.Contracts;
using ChessCake.Models.Positions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Engines.Contracts {
    public interface IEngine {
        public IBoard Board { get; }

        public IPlayer CurrentPlayer { get;  }

        public IList<ICell> LegalMoves(IPosition sourcePosition);

        public IDictionary<IPlayer, IList<BasePiece>> CapturedPieces { get; }

        public int Turn { get; }

        public bool IsThereOpponentPiece(ICell cell);

    }
}
