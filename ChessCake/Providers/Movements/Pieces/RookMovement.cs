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
    public class RookMovement : BasePieceMovement {

        public RookMovement(IEngine engine, ICell source) : base(engine, source) { }

        public override IList<ICell> GenerateLegalMoves() {
            IList<ICell> legalMoves = new List<ICell>();
            ICell referenceCell;

            int referenceRow;
            int referenceColumn;

            // North Direction:
            referenceRow = Source.Position.Row - 1;
            referenceColumn = Source.Position.Column;
            referenceCell = LoadReferenceCell(referenceRow, referenceColumn);

            while (ValidateReferenceCell(referenceCell)) {
                if (!referenceCell.IsOccupied()) {
                    legalMoves.Add(referenceCell);
                }

                referenceRow--;

                if (!Position.IsValidCoordinates(referenceRow, referenceColumn)) break;

                referenceCell = LoadReferenceCell(referenceRow, referenceColumn);

            }
            if (ValidateBreakCell(referenceCell)) {
                legalMoves.Add(referenceCell);

            }

            // East Direction:
            referenceRow = Source.Position.Row;
            referenceColumn = Source.Position.Column + 1;
            referenceCell = LoadReferenceCell(referenceRow, referenceColumn);
            while (ValidateReferenceCell(referenceCell)) {
                if (!referenceCell.IsOccupied()) {
                    legalMoves.Add(referenceCell);
                }

                referenceColumn++;

                if (!Position.IsValidCoordinates(referenceRow, referenceColumn)) break;

                referenceCell = LoadReferenceCell(referenceRow, referenceColumn);
            }
            if (ValidateBreakCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            // South Direction:
            referenceRow = Source.Position.Row + 1;
            referenceColumn = Source.Position.Column;
            referenceCell = LoadReferenceCell(referenceRow, referenceColumn);
            while (ValidateReferenceCell(referenceCell)) {
                if (!referenceCell.IsOccupied()) {
                    legalMoves.Add(referenceCell);
                }

                referenceRow++;

                if (!Position.IsValidCoordinates(referenceRow, referenceColumn)) break;

                referenceCell = LoadReferenceCell(referenceRow, referenceColumn);
            }
            if (ValidateBreakCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            // West Direction:
            referenceRow = Source.Position.Row;
            referenceColumn = Source.Position.Column - 1;
            referenceCell = LoadReferenceCell(referenceRow, referenceColumn);
            while (ValidateReferenceCell(referenceCell)) {
                if (!referenceCell.IsOccupied()) {
                    legalMoves.Add(referenceCell);
                }

                referenceColumn--;

                if (!Position.IsValidCoordinates(referenceRow, referenceColumn)) break;

                referenceCell = LoadReferenceCell(referenceRow, referenceColumn);

            }
            if (ValidateBreakCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            return legalMoves;

        }

        private ICell LoadReferenceCell(int referenceRow, int referenceColumn) {
            if (!Position.IsValidCoordinates(referenceRow, referenceColumn)) return null;

            ICell referenceCell = Engine.Board.GetCell(referenceRow, referenceColumn);

            return referenceCell;

        }

    }
}
