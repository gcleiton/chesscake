using ChessCake.Commons;
using ChessCake.Engines.Contracts;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Positions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ChessCake.Providers.Movements.Pieces.Contracts {
    public abstract class BasePieceMovement : IPieceMovement {

        public IEngine Engine { get; private set; }

        public BasePieceMovement(IEngine engine) {
            Engine = engine;
        }

        public abstract IList<ICell> GenerateLegalMoves(ICell source);

        protected bool ValidateReferenceCell(ICell cell) {
            return !Common.IsObjectNull(cell) && Position.IsValidPosition(cell.Position) && !Engine.Board.ThereIsAPiece(cell);

        }

        protected bool ValidateBreakCell(ICell cell) {
            return !Common.IsObjectNull(cell) && Position.IsValidPosition(cell.Position) && Engine.IsThereOpponentPiece(cell);

        }
    }
}
