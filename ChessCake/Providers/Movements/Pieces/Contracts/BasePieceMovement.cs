using ChessCake.Commons;
using ChessCake.Engines.Contracts;
using ChessCake.Models.Boards;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Positions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ChessCake.Providers.Movements.Pieces.Contracts {
    public abstract class BasePieceMovement : IPieceMovement {

        public IEngine Engine { get; private set; }
        public ICell Source { get; private set; }

        public BasePieceMovement(IEngine engine, ICell source) {
            Engine = engine;
            Source = source;
        }

        public abstract IList<ICell> GenerateLegalMoves();

        protected bool ValidateReferenceCell(ICell cell) {
            return !Common.IsObjectNull(cell) && Position.IsValidPosition(cell.Position) && !Engine.Board.ThereIsAPiece(cell);

        }

        protected bool ValidateBreakCell(ICell cell) {
            return !Common.IsObjectNull(cell) && Position.IsValidPosition(cell.Position) && IsThereOpponentPiece(cell);

        }

        protected bool IsThereOpponentPiece(ICell cell) {
            BasePiece piece = Engine.Board.FindPiece(cell.Position);

            return piece != null && piece.Color != Source.Piece.Color;
        }
    }
}
