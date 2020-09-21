using ChessCake.Commons;
using ChessCake.Commons.Enumerations;
using ChessCake.Commons.Factories;
using ChessCake.Engines.Contracts;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Players.Contracts;
using ChessCake.Providers.Movements.Contracts;
using ChessCake.Providers.Movements.Pieces.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Providers.Movements {
    class MovementProvider : IMovementProvider {

        public IEngine Engine { get; set; }

        public MovementProvider(IEngine engine) {
            Engine = engine;
        }

        public IList<ICell> GenerateLegalMoves(ICell source) {
            BasePiece piece = source.Piece;

            IList<ICell> legalMoves = new List<ICell>();

            if (Common.IsObjectNull(piece)) return legalMoves;

            switch (piece.Type) {
                case PieceType.PAWN:
                    legalMoves = GeneratePawnMovements(source);
                    break;

                case PieceType.KNIGHT:
                    legalMoves = GenerateKnightMovements(source);
                    break;

                case PieceType.BISHOP:
                    legalMoves = GenerateBishopMovements(source);
                    break;

                case PieceType.ROOK:
                    legalMoves = GenerateRookMovements(source);
                    break;

                case PieceType.QUEEN:
                    legalMoves = GenerateQueenMovements(source);
                    break;

                case PieceType.KING:
                    legalMoves = GenerateKingMovements(source);
                    break;

                default:
                    return legalMoves;

            }

            return legalMoves;

        }

        public IList<ICell> GeneratePawnMovements(ICell source) {
            IPieceMovement pawnMovement = MovementGeneratorFactory.CreatePawnMovement(Engine, source);

            return pawnMovement.GenerateLegalMoves();
        }
        public IList<ICell> GenerateKnightMovements(ICell source) {
            IPieceMovement knightMovement = MovementGeneratorFactory.CreateKnightMovement(Engine, source);

            return knightMovement.GenerateLegalMoves();
        }

        public IList<ICell> GenerateBishopMovements(ICell source) {
            IPieceMovement bishopMovement = MovementGeneratorFactory.CreateBishopMovement(Engine, source);

            return bishopMovement.GenerateLegalMoves();

        }

        public IList<ICell> GenerateRookMovements(ICell source) {
            IPieceMovement rookMovements = MovementGeneratorFactory.CreateRookMovement(Engine, source);

            return rookMovements.GenerateLegalMoves();
        }
        public IList<ICell> GenerateQueenMovements(ICell source) {
            IPieceMovement queenMovement = MovementGeneratorFactory.CreateQueenMovement(Engine, source);

            return queenMovement.GenerateLegalMoves();
        }
        public IList<ICell> GenerateKingMovements(ICell source) {
            IPieceMovement kingMovement = MovementGeneratorFactory.CreateKingMovement(Engine, source);

            return kingMovement.GenerateLegalMoves();
        }

        public bool IsLegalMovement(ICell source, ICell target) {
            return GenerateLegalMoves(source).Contains(target);
        }

        public bool IsThereAnyLegalMove(ICell source) {

            return GenerateLegalMoves(source).Count != 0;
        }

        public void Update(IEngine engine) {
            Engine = engine;
        }

        public void UpdateCurrentPlayer(IPlayer player) {
            Engine.CurrentPlayer = player;
        }

    }

}
