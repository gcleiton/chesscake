using ChessCake.Commons.Enumerations;
using ChessCake.Engines.Contracts;
using ChessCake.Models.Boards;
using ChessCake.Models.Boards.Cells;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Movements;
using ChessCake.Models.Movements.Contracts;
using ChessCake.Models.Pieces;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Players;
using ChessCake.Models.Players.Contracts;
using ChessCake.Models.Positions;
using ChessCake.Models.Positions.Chess;
using ChessCake.Models.Positions.Contracts;
using ChessCake.Providers.Movements;
using ChessCake.Providers.Movements.Contracts;
using ChessCake.Providers.Movements.Pieces;
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

        public static IMovement CreateMovement(ICell source, ICell target, IPlayer player) {
            return new Movement(source, target, player);
        }

        public static BasePiece CreatePiece(PieceType type, ChessColor color, IPosition position) {
            BasePiece piece = null;
            switch (type) {
                case PieceType.PAWN:
                    piece = new Pawn(color, position);
                    break;

                case PieceType.KNIGHT:
                    piece = new Knight(color, position);
                    break;

                case PieceType.BISHOP:
                    piece = new Bishop(color, position);
                    break;

                case PieceType.ROOK:
                    piece = new Rook(color, position);
                    break;

                case PieceType.QUEEN:
                    piece = new Queen(color, position);
                    break;

                case PieceType.KING:
                    piece = new King(color, position);
                    break;

                default:
                    piece = null;
                    break;

            }

            return piece;
        }
    }
}
