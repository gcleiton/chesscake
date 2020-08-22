using ChessCake.Commons;
using ChessCake.Commons.Enumerations;
using ChessCake.Engines.Contracts;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Boards.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Positions;
using ChessCake.Providers.Movements.Pieces.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ChessCake.Providers.Movements.Pieces {
    public class KingMovement : BasePieceMovement {

        private IPiece Piece; // King reference

        public KingMovement(IEngine engine, IPiece piece) : base(engine) {
            Piece = piece;
        }

        public override IList<ICell> GenerateLegalMoves(ICell source) {
            IList<ICell> legalMoves = new List<ICell>();
            ICell referenceCell;

            // North Direction
            referenceCell = LoadReferenceCell(source.Position.Row - 1, source.Position.Column);
            if (ValidateReferenceCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            // Northeast Direction
            referenceCell = LoadReferenceCell(source.Position.Row + 1, source.Position.Column + 1);
            if (ValidateReferenceCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            // East Direction
            referenceCell = LoadReferenceCell(source.Position.Row, source.Position.Column + 1);
            if (ValidateReferenceCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            // Southeast Direction
            referenceCell = LoadReferenceCell(source.Position.Row - 1, source.Position.Column + 1);
            if (ValidateReferenceCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            // South Direction
            referenceCell = LoadReferenceCell(source.Position.Row + 1, source.Position.Column);
            if (ValidateReferenceCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            // Southwest Direction
            referenceCell = LoadReferenceCell(source.Position.Row + 1, source.Position.Column - 1);
            if (ValidateReferenceCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            // West Direction
            referenceCell = LoadReferenceCell(source.Position.Row, source.Position.Column - 1);
            if (ValidateReferenceCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            // Northwest Direction
            referenceCell = LoadReferenceCell(source.Position.Row - 1, source.Position.Column - 1);
            if (ValidateReferenceCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            // Special Move - Castling:
            referenceCell = source;

            if (referenceCell.Piece.Type == PieceType.KING && Piece.MoveCount == 0 && !Engine.InCheck) {
                // Right rook:

                if (CanRightCastling(referenceCell)) {

                    ICell possibleMove = Engine.Board.FindNeighbor(referenceCell, 2, GridCoordinate.COLUMN);
                    legalMoves.Add(possibleMove);

                }

                if (CanLeftCastling(referenceCell)) {
                    ICell possibleMove = Engine.Board.FindNeighbor(referenceCell, -2, GridCoordinate.COLUMN);
                    legalMoves.Add(possibleMove);

                }

            }

            return legalMoves;

        }

        private ICell LoadReferenceCell(int referenceRow, int referenceColumn) {
            if (!Position.IsValidCoordinates(referenceRow, referenceColumn)) return null;

            ICell referenceCell = Engine.Board.GetCell(referenceRow, referenceColumn);

            return referenceCell;

        }

        private bool CanRightCastling(ICell kingCell) {

            // Rei pegando a posição (7, 7)
            IPiece rook = Engine.Board.GetCell(kingCell.Piece.Position.Row, kingCell.Piece.Position.Column + 3).Piece; // problema aqui

            ICell firstNeighbor = Engine.Board.FindNeighbor(kingCell, 1, GridCoordinate.COLUMN);

            ICell secondNeighbor = Engine.Board.FindNeighbor(kingCell, 2, GridCoordinate.COLUMN);

            return !Common.IsObjectNull(rook) && rook.Type == PieceType.ROOK &&
                     rook.Color == Piece.Color && Piece.MoveCount == 0 &&
                     Common.IsObjectNull(firstNeighbor.Piece) && Common.IsObjectNull(secondNeighbor.Piece);
        
        }

        private bool CanLeftCastling(ICell kingCell) {
            IPiece rook = Engine.Board.GetCell(kingCell.Piece.Position.Row, kingCell.Piece.Position.Column - 4).Piece;

            ICell firstNeighbor = Engine.Board.FindNeighbor(kingCell, -1, GridCoordinate.COLUMN);
            ICell secondNeighbor = Engine.Board.FindNeighbor(kingCell, -2, GridCoordinate.COLUMN);
            ICell thirdNeighbor = Engine.Board.FindNeighbor(kingCell, -3, GridCoordinate.COLUMN);

            return !Common.IsObjectNull(rook) && rook.Type == PieceType.ROOK && rook.Color == Piece.Color && Piece.MoveCount == 0 &&
                     Common.IsObjectNull(firstNeighbor.Piece) && Common.IsObjectNull(secondNeighbor.Piece) && Common.IsObjectNull(thirdNeighbor.Piece);

        }

    }
}
