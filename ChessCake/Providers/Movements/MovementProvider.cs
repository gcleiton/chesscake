using ChessCake.Commons;
using ChessCake.Commons.Enumerations;
using ChessCake.Commons.Factories;
using ChessCake.Engines.Contracts;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Providers.Movements.Contracts;
using ChessCake.Providers.Movements.Pieces.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Providers.Movements {
    class MovementProvider : IMovementProvider {

        private IEngine Engine;

        public MovementProvider(IEngine engine) {
            Engine = engine;
        }

        public IList<ICell> GenerateLegalMoves(ICell source) {
            BasePiece piece = source.Piece;

            if (Common.IsObjectNull(piece)) return null;

            IList<ICell> legalMoves = new List<ICell>();
            
            switch (piece.Type) {
                case PieceType.PAWN:
                    break;

                case PieceType.KNIGHT:
                    break;

                case PieceType.BISHOP:
                    legalMoves = GenerateBishopMovements(source);
                    break;

                case PieceType.ROOK:
                    break;

                case PieceType.QUEEN:
                    break;

                case PieceType.KING:
                    break;

                default:
                    break;
            }

            return legalMoves;

        }

        

        public IList<ICell> GeneratePawnMovements(ICell source) {
            return null;
        }
        public IList<ICell> GenerateKnightMovements(ICell source) {
            return null;
        }

        public IList<ICell> GenerateBishopMovements(ICell source) {
            IPieceMovement bishopMovement = MovementGeneratorFactory.CreateBishopMovement(Engine);

            return bishopMovement.GenerateLegalMoves(source);

        }

        public IList<ICell> GenerateRookMovements(ICell source) {
            return null;
        }
        public IList<ICell> GenerateQueenMovements(ICell source) {
            return null;
        }
        public IList<ICell> GenerateKingMovements(ICell source) {
            return null;
        }

        public bool IsLegalMovement(ICell source, ICell target) {
            return GenerateLegalMoves(source).Contains(target);
        }

        public bool IsThereAnyLegalMove(ICell source) {
            return GenerateLegalMoves(source).Count != 0;
        }

        public void Update(IEngine engine) {
            this.Engine = engine;
        }

    }

}
