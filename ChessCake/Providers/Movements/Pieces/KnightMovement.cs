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
    public class KnightMovement : BasePieceMovement {

        public KnightMovement(IEngine engine, ICell source) : base(engine, source) { }

        public override IList<ICell> GenerateLegalMoves() {
            IList<ICell> legalMoves = new List<ICell>();
            ICell referenceCell;

            // Northeast Direction:
            referenceCell = LoadReferenceCell(Source.Position.Row - 2, Source.Position.Column + 1);
            if (ValidateReferenceCell(referenceCell) || ValidateBreakCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            referenceCell = LoadReferenceCell(Source.Position.Row - 1, Source.Position.Column + 2);
            if (ValidateReferenceCell(referenceCell) || ValidateBreakCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            // Southeast Direction:
            referenceCell = LoadReferenceCell(Source.Position.Row + 1, Source.Position.Column + 2);
            if (ValidateReferenceCell(referenceCell) || ValidateBreakCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            referenceCell = LoadReferenceCell(Source.Position.Row + 2, Source.Position.Column + 1);
            if (ValidateReferenceCell(referenceCell) || ValidateBreakCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            // Southwest Direction:
            referenceCell = LoadReferenceCell(Source.Position.Row + 2, Source.Position.Column - 1);
            if (ValidateReferenceCell(referenceCell) || ValidateBreakCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            referenceCell = LoadReferenceCell(Source.Position.Row + 1, Source.Position.Column - 2);
            if (ValidateReferenceCell(referenceCell) || ValidateBreakCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            // Northwest Direction:
            referenceCell = LoadReferenceCell(Source.Position.Row - 1, Source.Position.Column - 2);
            if (ValidateReferenceCell(referenceCell) || ValidateBreakCell(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            referenceCell = LoadReferenceCell(Source.Position.Row - 2, Source.Position.Column - 1);
            if (ValidateReferenceCell(referenceCell) || ValidateBreakCell(referenceCell)) {
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
