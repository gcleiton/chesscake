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
    public class BishopMovement : IPieceMovement {

        private IEngine Engine;

        public BishopMovement(IEngine engine) {
            Engine = engine;
        }

        public IList<ICell> GenerateLegalMoves(ICell source) {
            IList<ICell> legalMoves = new List<ICell>();
            ICell referenceCell;

            int referenceRow;
            int referenceColumn;

            referenceRow = source.Position.Row - 1;
            referenceColumn = source.Position.Column - 1;
            referenceCell = LoadReferenceCell(referenceRow, referenceColumn);

            while (!(referenceCell is null) && Position.IsValidPosition(referenceCell.Position)) {
                if (!referenceCell.IsOccupied()) {
                    legalMoves.Add(referenceCell);
                }

                referenceRow--;
                referenceColumn--;

                if (!Position.IsValidCoordinates(referenceRow, referenceColumn)) break;

                referenceCell = LoadReferenceCell(referenceRow, referenceColumn);

            }
            if (!(referenceCell is null) && Engine.IsThereOpponentPiece(referenceCell)) {

                legalMoves.Add(referenceCell);
            }


            referenceRow = source.Position.Row - 1;
            referenceColumn = source.Position.Column + 1;
            referenceCell = LoadReferenceCell(referenceRow, referenceColumn);
            while (!(referenceCell is null) && Position.IsValidPosition(referenceCell.Position)) {
                if (!referenceCell.IsOccupied()) {
                    legalMoves.Add(referenceCell);
                }

                referenceRow--;
                referenceColumn++;

                if (!Position.IsValidCoordinates(referenceRow, referenceColumn)) break;

                referenceCell = LoadReferenceCell(referenceRow, referenceColumn);
            }
            if (referenceCell != null && Engine.IsThereOpponentPiece(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            referenceRow = source.Position.Row + 1;
            referenceColumn = source.Position.Column + 1;
            referenceCell = LoadReferenceCell(referenceRow, referenceColumn);
            while (!(referenceCell is null) && Position.IsValidPosition(referenceCell.Position)) {
                if (!referenceCell.IsOccupied()) {
                    legalMoves.Add(referenceCell);
                }

                referenceRow++;
                referenceColumn++;

                if (!Position.IsValidCoordinates(referenceRow, referenceColumn)) break;

                referenceCell = LoadReferenceCell(referenceRow, referenceColumn);
            }
            if (referenceCell != null && Engine.IsThereOpponentPiece(referenceCell)) {
                legalMoves.Add(referenceCell);
            }

            referenceRow = source.Position.Row + 1;
            referenceColumn = source.Position.Column - 1;
            referenceCell = LoadReferenceCell(referenceRow, referenceColumn);
            while (!(referenceCell is null) && Position.IsValidPosition(referenceCell.Position)) {
                if (!referenceCell.IsOccupied()) {
                    legalMoves.Add(referenceCell);
                }

                referenceRow++;
                referenceColumn--;

                if (!Position.IsValidCoordinates(referenceRow, referenceColumn)) break;

                referenceCell = LoadReferenceCell(referenceRow, referenceColumn);

            }
            if (referenceCell != null && Engine.IsThereOpponentPiece(referenceCell)) {
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
