using ChessCake.Commons.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Commons {
    class GameConstants {
        public static readonly IEnumerable<PieceType> MAJOR_PIECES_SEQUENCE = new List<PieceType>
            {
                PieceType.ROOK,
                PieceType.KNIGHT,
                PieceType.BISHOP,
                PieceType.QUEEN,
                PieceType.KING,
                PieceType.BISHOP,
                PieceType.KNIGHT,
                PieceType.ROOK,
            };

        public const int INITIAL_MAJOR_ROW_OF_FIRST_PLAYER = 8;
        public const int INITIAL_PAWNS_ROW_OF_FIRST_PLAYER = 7;

        public const int INITIAL_MAJOR_ROW_OF_SECOND_PLAYER = 1;
        public const int INITIAL_PAWNS_ROW_OF_SECOND_PLAYER = 2;

    }
}
