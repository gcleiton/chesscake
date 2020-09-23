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

        public KingMovement(IEngine engine, ICell source) : base(engine, source) { }

        public override IList<ICell> GenerateLegalMoves() {
            IList<ICell> legalMoves = new List<ICell>();
            ICell referenceCell;

            // North Direction
            referenceCell = LoadReferenceCell(Source.Position.Row - 1, Source.Position.Column);
            if (ValidateReferenceCell(referenceCell) || ValidateBreakCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            // Northeast Direction
            referenceCell = LoadReferenceCell(Source.Position.Row + 1, Source.Position.Column + 1);
            if (ValidateReferenceCell(referenceCell) || ValidateBreakCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            // East Direction
            referenceCell = LoadReferenceCell(Source.Position.Row, Source.Position.Column + 1);
            if (ValidateReferenceCell(referenceCell) || ValidateBreakCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            // Southeast Direction
            referenceCell = LoadReferenceCell(Source.Position.Row - 1, Source.Position.Column + 1);
            if (ValidateReferenceCell(referenceCell) || ValidateBreakCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            // South Direction
            referenceCell = LoadReferenceCell(Source.Position.Row + 1, Source.Position.Column);
            if (ValidateReferenceCell(referenceCell) || ValidateBreakCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            // Southwest Direction
            referenceCell = LoadReferenceCell(Source.Position.Row + 1, Source.Position.Column - 1);
            if (ValidateReferenceCell(referenceCell) || ValidateBreakCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            // West Direction
            referenceCell = LoadReferenceCell(Source.Position.Row, Source.Position.Column - 1);
            if (ValidateReferenceCell(referenceCell) || ValidateBreakCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            // Northwest Direction
            referenceCell = LoadReferenceCell(Source.Position.Row - 1, Source.Position.Column - 1);
            if (ValidateReferenceCell(referenceCell) || ValidateBreakCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            // Special Move - Castling:
            referenceCell = Source;

            if (CheckCastlingConditions(referenceCell)) {
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

        private bool CheckCastlingConditions(ICell referenceCell) {

            return referenceCell.Piece.Type == PieceType.KING && Source.Piece.MoveCount == 0 && !(Engine.Status == GameStatus.CHECK);

        }

        private ICell LoadReferenceCell(int referenceRow, int referenceColumn) {
            if (!Position.IsValidCoordinates(referenceRow, referenceColumn)) return null;

            ICell referenceCell = Engine.Board.GetCell(referenceRow, referenceColumn);

            return referenceCell;

        }

        private bool CanRightCastling(ICell kingCell) {

            ICell rookCell = Engine.Board.GetCell(kingCell.Piece.Position.Row, kingCell.Piece.Position.Column + 3); // problema aqui

            if (Common.IsObjectNull(rookCell)) return false;

            ICell firstNeighbor = Engine.Board.FindNeighbor(kingCell, 1, GridCoordinate.COLUMN);

            ICell secondNeighbor = Engine.Board.FindNeighbor(kingCell, 2, GridCoordinate.COLUMN);

            return !Common.IsObjectNull(rookCell.Piece) && rookCell.Piece.Type == PieceType.ROOK &&
                     rookCell.Piece.Color == Source.Piece.Color && Source.Piece.MoveCount == 0 &&
                     Common.IsObjectNull(firstNeighbor.Piece) && Common.IsObjectNull(secondNeighbor.Piece);
        
        }

        private bool CanLeftCastling(ICell kingCell) {
            ICell rookCell = Engine.Board.GetCell(kingCell.Piece.Position.Row, kingCell.Piece.Position.Column - 4);

            if (Common.IsObjectNull(rookCell)) return false;

            ICell firstNeighbor = Engine.Board.FindNeighbor(kingCell, -1, GridCoordinate.COLUMN);
            ICell secondNeighbor = Engine.Board.FindNeighbor(kingCell, -2, GridCoordinate.COLUMN);
            ICell thirdNeighbor = Engine.Board.FindNeighbor(kingCell, -3, GridCoordinate.COLUMN);

            return !Common.IsObjectNull(rookCell.Piece) && rookCell.Piece.Type == PieceType.ROOK && rookCell.Piece.Color == Source.Piece.Color && Source.Piece.MoveCount == 0 &&
                     Common.IsObjectNull(firstNeighbor.Piece) && Common.IsObjectNull(secondNeighbor.Piece) && Common.IsObjectNull(thirdNeighbor.Piece);

        }

    }
}
