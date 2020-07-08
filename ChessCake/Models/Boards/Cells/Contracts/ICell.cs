using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Positions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Boards.Cells.Contracts {
    interface ICell {
        public Position Position { get; }
        public BasePiece Piece { get; }

        public bool isOccupied();

        public void placePiece(BasePiece piece);

        public BasePiece releasePiece();

    }
}
