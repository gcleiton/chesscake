﻿using ChessCake.Commons;
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

        public IEngine Engine { get; set; }

        public MovementProvider(IEngine engine) {
            Engine = engine;
        }

        public IList<ICell> GenerateLegalMoves(ICell source) {
            BasePiece piece = source.Piece;

            if (Common.IsObjectNull(piece)) return null;

            IList<ICell> legalMoves = new List<ICell>();
            
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
                    break;
            }

            return legalMoves;

        }

        public IList<ICell> GeneratePawnMovements(ICell source) {
            IPieceMovement pawnMovement = MovementGeneratorFactory.CreatePawnMovement(Engine);

            return pawnMovement.GenerateLegalMoves(source);
        }
        public IList<ICell> GenerateKnightMovements(ICell source) {
            IPieceMovement knightMovement = MovementGeneratorFactory.CreateKnightMovement(Engine);

            return knightMovement.GenerateLegalMoves(source);
        }

        public IList<ICell> GenerateBishopMovements(ICell source) {
            IPieceMovement bishopMovement = MovementGeneratorFactory.CreateBishopMovement(Engine);

            return bishopMovement.GenerateLegalMoves(source);

        }

        public IList<ICell> GenerateRookMovements(ICell source) {
            IPieceMovement rookMovements = MovementGeneratorFactory.CreateRookMovement(Engine);

            return rookMovements.GenerateLegalMoves(source);
        }
        public IList<ICell> GenerateQueenMovements(ICell source) {
            IPieceMovement queenMovement = MovementGeneratorFactory.CreateQueenMovement(Engine);

            return queenMovement.GenerateLegalMoves(source);
        }
        public IList<ICell> GenerateKingMovements(ICell source) {
            IPieceMovement kingMovement = MovementGeneratorFactory.CreateKingMovement(Engine);

            return kingMovement.GenerateLegalMoves(source);
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

    }

}
