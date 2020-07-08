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

        public BasePiece findPiece(int row, int column);

        public BasePiece findPiece(Position position);

        public BasePiece findPiece(Cell cell);

        public Cell getCell(Position position);

        public Cell getCell(int row, int column);

        public void placePiece(BasePiece piece, Cell cell);

        public BasePiece removePiece(Position position);

    }
}
