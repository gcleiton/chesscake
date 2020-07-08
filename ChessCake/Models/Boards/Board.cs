using ChessCake.Commons;
using ChessCake.Exceptions;
using ChessCake.Models.Boards.Cells;
using ChessCake.Models.Boards.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Positions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Boards {
    public class Board : IBoard {
        public Cell[,] Grid { get; private set; }
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

        public BasePiece findPiece(int row, int column) {
            if (!Position.isValidCoordinates(row, column)) {
                throw new ChessException("Coordenadas da posição inválida!");
            }
            return Grid[row, column].Piece;
        }

        public BasePiece findPiece(Position position) {
            return Grid[position.Row, position.Column].Piece;
        }

        public BasePiece findPiece(Cell cell) {
            return Grid[cell.Position.Row, cell.Position.Row].Piece;
        }

        public Cell getCell(Position position) {
            if (Position.isValidCoordinates(position.Row, position.Column)) {
                throw new BoardException("Coordenadas da posição inválida!");
            }

            return Grid[position.Row, position.Column];
        }

        public Cell getCell(int row, int column) {
            if (!Position.isValidCoordinates(row, column)) {
                throw new BoardException("Coordenadas da posição inválida!");
            }

            return Grid[row, column];
        }

        public void placePiece(BasePiece piece, Cell cell) {
            Grid[cell.Position.Row, cell.Position.Column].placePiece(piece);
        }

        public BasePiece removePiece(Position position) {
            Cell cell = getCell(position);
            BasePiece releasedPiece = cell.releasePiece();

            Grid[position.Row, position.Column] = cell;

            return releasedPiece;
        }


    }
}
