﻿using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Positions;
using ChessCake.Models.Positions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Boards.Cells.Contracts {
    public interface ICell {
        public IPosition Position { get; }
        public BasePiece Piece { get; }

        public bool IsOccupied();

        public void PlacePiece(BasePiece piece);

        public BasePiece ReleasePiece();

    }
}
