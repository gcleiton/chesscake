using ChessCake.Models.Boards.Cells;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Positions;
using ChessCake.Models.Positions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Boards.Contracts {
    public interface IBoard {
        public ICell[,] Grid { get; }
        public int Rows { get; }
        public int Columns { get; }

        public BasePiece FindPiece(int row, int column);

        public BasePiece FindPiece(IPosition position);

        public BasePiece FindPiece(ICell cell);

        public ICell GetCell(IPosition position);

        public ICell GetCell(int row, int column);

        public void PlacePiece(BasePiece piece, ICell cell);

        public BasePiece RemovePiece(IPosition position);

        public bool ThereIsAPiece(IPosition position);

        public bool ThereIsAPiece(ICell cell);

    }
}
