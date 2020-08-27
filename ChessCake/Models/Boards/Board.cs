using ChessCake.Commons;
using ChessCake.Commons.Constants;
using ChessCake.Commons.Enumerations;
using ChessCake.Exceptions;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Boards.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Positions;
using ChessCake.Models.Positions.Contracts;
using System;

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
            return Grid[cell.Position.Row, cell.Position.Column].Piece;
        }

        public ICell GetCell(IPosition position) {
            return GetCell(position.Row, position.Column);

        }

        public ICell GetCell(int row, int column) {
            if (!Position.IsValidCoordinates(row, column)) {
                return null;
                throw new BoardException("Coordenadas da posição inválida!");
            }

            return Grid[row, column];
        }

        public void PlacePiece(BasePiece piece, ICell cell) {
            // Verificar o motivo do disparo da exceção durante a alocação das peças
            //if(ThereIsAPiece(cell)) {
            //    throw new BoardException("There is already a piece on position " + cell.Position);
            //}
            piece.Position = cell.Position;
            Grid[cell.Position.Row, cell.Position.Column].PlacePiece(piece);
        }

        public BasePiece RemovePiece(IPosition position) {
            ICell cell = GetCell(position);
            BasePiece releasedPiece = cell.ReleasePiece();

            Grid[position.Row, position.Column] = cell;

            return releasedPiece;
        }

        public BasePiece RemovePiece(ICell cell) {
            return RemovePiece(cell.Position);

        }

        public ICell FindNeighbor(BasePiece referencePiece, int offset, GridCoordinate coordinate) {
            IPosition position = ChessFactory.CreatePosition(0, 0);

            if (coordinate == GridCoordinate.ROW) {
                position.SetCoordinates(referencePiece.Position.Row + offset, referencePiece.Position.Column);
                return GetCell(position);

            }

            position.SetCoordinates(referencePiece.Position.Row, referencePiece.Position.Column + offset);

            return GetCell(position);

        }

        public ICell FindNeighbor(ICell referenceCell, int offset, GridCoordinate coordinate) {
            return FindNeighbor(referenceCell.Piece, offset, coordinate);

        }

        public bool ThereIsAPiece(IPosition position) {
            if (!Position.IsValidPosition(position)) {
                throw new BoardException("Invalid Position");
            }
            return !Common.IsObjectNull(FindPiece(position));
        }

        public bool ThereIsAPiece(ICell cell) {
            if (!Position.IsValidPosition(cell.Position)) {
                throw new BoardException("Invalid Position");
            }
            return !Common.IsObjectNull(FindPiece(cell));
        }

    }
}
