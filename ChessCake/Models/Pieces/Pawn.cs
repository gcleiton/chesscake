using ChessCake.Commons.Enumerations;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Positions.Contracts;

namespace ChessCake.Models.Pieces {
    class Pawn: BasePiece {

        public Pawn(ChessColor color, IPosition position) : base(color, position) {
            Type = PieceType.PAWN;
        }

        public override string ToString() {
            return "P";
        }

    }
}
