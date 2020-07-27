using ChessCake.Commons.Enumerations;
using ChessCake.Models.Boards;
using ChessCake.Models.Boards.Cells;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Movements;
using ChessCake.Models.Movements.Contracts;
using ChessCake.Models.Pieces;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Players;
using ChessCake.Models.Positions;
using ChessCake.Models.Positions.Chess;
using ChessCake.Models.Positions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Commons {
    public static class ChessFactory {

        public static Board CreateBoard() {
            return new Board();
        }

        public static Cell CreateCell(Position position, BasePiece piece) {
            return new Cell(position, piece);
        }
        public static Cell[,] CreateCellMatrix(int rows, int columns) {
            return new Cell[rows, columns];
        }

        public static Position CreatePosition(int row, int column) {
            return new Position(row, column);
        }

        public static ChessPosition CreateChessPosition(char column, int row) {
            return new ChessPosition(column, row);
        }

        public static Player CreatePlayer(string name, ChessColor color) {
            return new Player(name, color);
        }

        public static IMovement CreateMovement(ICell source, ICell target) {
            return new Movement(source, target);
        }

        public static BasePiece CreatePiece(PieceType type, ChessColor color) {
            BasePiece piece = null;
            switch (type) {
                case PieceType.PAWN:
                    piece = new Pawn(color);
                    break;

                case PieceType.KNIGHT:
                    piece = new Knight(color);
                    break;

                case PieceType.BISHOP:
                    piece = new Bishop(color);
                    break;

                case PieceType.ROOK:
                    piece = new Rook(color);
                    break;

                case PieceType.QUEEN:
                    piece = new Queen(color);
                    break;

                case PieceType.KING:
                    piece = new King(color);
                    break;

                case PieceType.EMPTY:
                    piece = new Empty(color);
                    break;

                default:
                    piece = null;
                    break;

            }

            return piece;
        }
    }
}
