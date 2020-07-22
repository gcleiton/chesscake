using ChessCake.Models.Boards.Cells;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Positions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Boards.Contracts {
    interface IBoard {
        public Cell[,] Grid { get; }
        public int Rows { get; }
        public int Columns { get; }

        public BasePiece FindPiece(int row, int column);

        public BasePiece FindPiece(Position position);

        public BasePiece FindPiece(Cell cell);

        public Cell GetCell(Position position);

        public Cell GetCell(int row, int column);

        public void PlacePiece(BasePiece piece, Cell cell);

        public BasePiece RemovePiece(Position position);

    }
}
