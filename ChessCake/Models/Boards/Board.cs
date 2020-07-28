using ChessCake.Commons;
using ChessCake.Exceptions;
using ChessCake.Models.Boards.Cells;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Boards.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Positions;
using ChessCake.Models.Positions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Boards {
    public class Board : IBoard {
        public ICell[,] Grid { get; private set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public Board() {
            this.Rows = GlobalConstants.STANDARD_BOARD_ROWS;
            this.Columns = GlobalConstants.STANDARD_BOARD_COLUMNS;

            Grid = ChessFactory.CreateCellMatrix(this.Rows, this.Columns);
            for (int i = 0; i < this.Rows; i++) {
                for (int j = 0; j < this.Columns; j++) {
                    Position position = ChessFactory.CreatePosition(i, j);
                    Grid[i, j] = ChessFactory.CreateCell(position, null);
                }
            }
        }

        public BasePiece FindPiece(int row, int column) {
            if (!Position.IsValidCoordinates(row, column)) {
                throw new ChessException("Coordenadas da posição inválida!");
            }
            return Grid[row, column].Piece;
        }

        public BasePiece FindPiece(IPosition position) {
            return Grid[position.Row, position.Column].Piece;
        }

        public BasePiece FindPiece(ICell cell) {
            return Grid[cell.Position.Row, cell.Position.Row].Piece;
        }

        public ICell GetCell(IPosition position) {
            if (!Position.IsValidCoordinates(position.Row, position.Column)) {
                throw new BoardException("Coordenadas da posição inválida!");
            }

            return Grid[position.Row, position.Column];
        }

        public ICell GetCell(int row, int column) {
            if (!Position.IsValidCoordinates(row, column)) {
                throw new BoardException("Coordenadas da posição inválida!");
            }

            return Grid[row, column];
        }

        public void PlacePiece(BasePiece piece, ICell cell) {
            Grid[cell.Position.Row, cell.Position.Column].placePiece(piece);
        }

        public BasePiece RemovePiece(IPosition position) {
            ICell cell = GetCell(position);
            BasePiece releasedPiece = cell.releasePiece();

            Grid[position.Row, position.Column] = cell;

            return releasedPiece;
        }


    }
}
