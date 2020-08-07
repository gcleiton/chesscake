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
    public class KingMovement : BasePieceMovement {

        public KingMovement(IEngine engine) : base(engine) { }

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

            return legalMoves;

        }

        private ICell LoadReferenceCell(int referenceRow, int referenceColumn) {
            if (!Position.IsValidCoordinates(referenceRow, referenceColumn)) return null;

            ICell referenceCell = Engine.Board.GetCell(referenceRow, referenceColumn);

            return referenceCell;

        }

    }
}
