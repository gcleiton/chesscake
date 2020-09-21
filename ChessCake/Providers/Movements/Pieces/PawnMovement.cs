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

        public PawnMovement(IEngine engine, ICell source) : base(engine, source) { }

        public override IList<ICell> GenerateLegalMoves() {
            IList<ICell> legalMoves = new List<ICell>();
            ICell referenceCell;
            BasePiece selectedPiece = Source.Piece;

            if (selectedPiece.Color == ChessColor.WHITE) {
                referenceCell = LoadReferenceCell(Source.Position.Row - 1, Source.Position.Column);
                if (ValidateReferenceCell(referenceCell)) {
                    legalMoves.Add(referenceCell);
                }

                referenceCell = LoadReferenceCell(Source.Position.Row - 2, Source.Position.Column);
                if (ValidateReferenceCell(referenceCell) && ValidateReferenceCell(referenceCell) && selectedPiece.MoveCount == 0) {
                    legalMoves.Add(referenceCell);
                }

                // Northeast Direction
                referenceCell = LoadReferenceCell(Source.Position.Row - 1, Source.Position.Column + 1);
                if (ValidateBreakCell(referenceCell)) {
                    legalMoves.Add(referenceCell);

                }

                // Northwest Direction
                referenceCell = LoadReferenceCell(Source.Position.Row - 1, Source.Position.Column - 1);
                if(ValidateBreakCell(referenceCell)) {
                    legalMoves.Add(referenceCell);

                }

                // Special move - enPassant

                if(selectedPiece.Position.Row == 3) {
                    ICell leftCell = Engine.Board.FindNeighbor(selectedPiece, -1, GridCoordinate.COLUMN);

                    ICell rightCell = Engine.Board.FindNeighbor(selectedPiece, 1, GridCoordinate.COLUMN);
                     
                    if (IsEnPassantMove(leftCell)) {
                        referenceCell = LoadReferenceCell(Source.Position.Row - 1, Source.Position.Column - 1);
                        legalMoves.Add(referenceCell);
                    }

                    if (IsEnPassantMove(rightCell)) {
                        referenceCell = LoadReferenceCell(Source.Position.Row - 1, Source.Position.Column + 1);

                        legalMoves.Add(referenceCell);
                    }

                }


            } else {
                referenceCell = LoadReferenceCell(Source.Position.Row + 1, Source.Position.Column);
                if (ValidateReferenceCell(referenceCell)) {
                    legalMoves.Add(referenceCell);
                }

                referenceCell = LoadReferenceCell(Source.Position.Row + 2, Source.Position.Column);
                if (ValidateReferenceCell(referenceCell) && ValidateReferenceCell(referenceCell) && selectedPiece.MoveCount == 0) {

                    legalMoves.Add(referenceCell);
                }

                // Northeast Direction
                referenceCell = LoadReferenceCell(Source.Position.Row + 1, Source.Position.Column + 1);
                if (ValidateBreakCell(referenceCell)) {
                    legalMoves.Add(referenceCell);

                }

                // Northwest Direction
                referenceCell = LoadReferenceCell(Source.Position.Row + 1, Source.Position.Column - 1);
                if (ValidateBreakCell(referenceCell)) {
                    legalMoves.Add(referenceCell);

                }

                if (selectedPiece.Position.Row == 4) {
                    ICell leftCell = Engine.Board.FindNeighbor(selectedPiece, -1, GridCoordinate.COLUMN);
                    ICell rightCell = Engine.Board.FindNeighbor(selectedPiece, 1, GridCoordinate.COLUMN);

                    if (IsEnPassantMove(leftCell)) {
                        referenceCell = LoadReferenceCell(Source.Position.Row + 1, Source.Position.Column - 1);

                        legalMoves.Add(referenceCell);
                    }

                    if (IsEnPassantMove(rightCell)) {
                        referenceCell = LoadReferenceCell(Source.Position.Row + 1, Source.Position.Column + 1);

                        legalMoves.Add(referenceCell);
                    }

                }
            }

            return legalMoves;

        }

        private ICell LoadReferenceCell(int referenceRow, int referenceColumn) {
            if (!Position.IsValidCoordinates(referenceRow, referenceColumn)) return null;

            ICell referenceCell = Engine.Board.GetCell(referenceRow, referenceColumn);

            return referenceCell;

        }

        private bool IsEnPassantMove(ICell cell) {
            return !Common.IsObjectNull(cell) && IsThereOpponentPiece(cell) && cell.Piece == Engine.EnPassant;

        }

    }
}
