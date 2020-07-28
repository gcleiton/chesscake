using ChessCake.Commons.Enumerations;
using ChessCake.Models.Pieces.Contracts;

namespace ChessCake.Models.Pieces {
    class Pawn: BasePiece {

        public const PieceType pieceType = PieceType.PAWN;
        public Pawn(ChessColor color) : base(color) {
            Type = PieceType.PAWN;
        }

        public override string ToString() {
            return "P";
        }

    }
}
