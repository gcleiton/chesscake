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
using System.Text;

namespace ChessCake.Providers.Movements.Pieces {
    public class PawnMovement : BasePieceMovement {

        public PawnMovement(IEngine engine) : base(engine) { }

        public override IList<ICell> GenerateLegalMoves(ICell source) {
            IList<ICell> legalMoves = new List<ICell>();
            ICell firstCell, secondCell, diagonalCell;
            IPiece selectedPiece = source.Piece;

            if (selectedPiece.Color == ChessColor.WHITE) {
                firstCell = LoadReferenceCell(source.Position.Row - 1, source.Position.Column);
                if (ValidateReferenceCell(firstCell)) {
                    legalMoves.Add(firstCell);
                }

                secondCell = LoadReferenceCell(source.Position.Row - 2, source.Position.Column);
                if (ValidateReferenceCell(secondCell) && ValidateReferenceCell(firstCell) && selectedPiece.MoveCount == 0) {
                    legalMoves.Add(secondCell);
                }

                // Northeast Direction
                diagonalCell = LoadReferenceCell(source.Position.Row - 1, source.Position.Column + 1);
                if (ValidateBreakCell(diagonalCell)) {
                    legalMoves.Add(diagonalCell);

                }

                // Northwest Direction
                diagonalCell = LoadReferenceCell(source.Position.Row - 1, source.Position.Column - 1);
                if(ValidateBreakCell(diagonalCell)) {
                    legalMoves.Add(diagonalCell);

                }

            } else {
                firstCell = LoadReferenceCell(source.Position.Row + 1, source.Position.Column);
                if (ValidateReferenceCell(firstCell)) {
                    legalMoves.Add(firstCell);
                }

                secondCell = LoadReferenceCell(source.Position.Row + 2, source.Position.Column);
                if (ValidateReferenceCell(secondCell) && ValidateReferenceCell(firstCell) && selectedPiece.MoveCount == 0) {
                    legalMoves.Add(secondCell);
                }

                // Northeast Direction
                diagonalCell = LoadReferenceCell(source.Position.Row + 1, source.Position.Column + 1);
                if (ValidateBreakCell(diagonalCell)) {
                    legalMoves.Add(diagonalCell);

                }

                // Northwest Direction
                diagonalCell = LoadReferenceCell(source.Position.Row + 1, source.Position.Column - 1);
                if (ValidateBreakCell(diagonalCell)) {
                    legalMoves.Add(diagonalCell);

                }
            }
            //} else {
            //    firstCell = LoadReferenceCell(source.Position.Row + 1, source.Position.Column);
            //    if (ValidateReferenceCell(firstCell)) {
            //        legalMoves.Add(firstCell);
            //    }

            //    secondCell = LoadReferenceCell(source.Position.Row + 2, source.Position.Column);
            //    if (selectedPiece.MoveCount == 0 && ValidateReferenceCell(secondCell)) {
            //        legalMoves.Add(secondCell);
            //    }
            //}

            return legalMoves;

        }

        private ICell LoadReferenceCell(int referenceRow, int referenceColumn) {
            if (!Position.IsValidCoordinates(referenceRow, referenceColumn)) return null;

            ICell referenceCell = Engine.Board.GetCell(referenceRow, referenceColumn);

            return referenceCell;

        }

    }
}
