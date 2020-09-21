using ChessCake.Engines.Contracts;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Providers.Movements.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Commons.Factories {
    class MovementGeneratorFactory {

        public static PawnMovement CreatePawnMovement(IEngine engine, ICell source) {
            return new PawnMovement(engine, source);
        }

        public static KnightMovement CreateKnightMovement(IEngine engine, ICell source) {
            return new KnightMovement(engine, source);
        }

        public static BishopMovement CreateBishopMovement(IEngine engine, ICell source) {
            return new BishopMovement(engine, source);
        }

        public static RookMovement CreateRookMovement(IEngine engine, ICell source) {
            return new RookMovement(engine, source);
        }

        public static QueenMovement CreateQueenMovement(IEngine engine, ICell source) {
            return new QueenMovement(engine, source);
        }

        public static KingMovement CreateKingMovement(IEngine engine, ICell source) {
            return new KingMovement(engine, source);
        }

    }
}
