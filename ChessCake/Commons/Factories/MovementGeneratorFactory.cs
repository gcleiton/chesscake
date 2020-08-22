using ChessCake.Engines.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Providers.Movements.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Commons.Factories {
    class MovementGeneratorFactory {

        public static PawnMovement CreatePawnMovement(IEngine engine) {
            return new PawnMovement(engine);
        }

        public static KnightMovement CreateKnightMovement(IEngine engine) {
            return new KnightMovement(engine);
        }

        public static BishopMovement CreateBishopMovement(IEngine engine) {
            return new BishopMovement(engine);
        }

        public static RookMovement CreateRookMovement(IEngine engine) {
            return new RookMovement(engine);
        }

        public static QueenMovement CreateQueenMovement(IEngine engine) {
            return new QueenMovement(engine);
        }

        public static KingMovement CreateKingMovement(IEngine engine, IPiece piece) {
            return new KingMovement(engine, piece);
        }

    }
}
