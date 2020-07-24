using ChessCake.Commons.Enumerations;
using ChessCake.Models.Pieces.Contracts;

namespace ChessCake.Models.Pieces {
    class Pawn: BasePiece {
        
        public Pawn(ChessColor color) : base(color) {

        }

        public override string ToString() {
            return "P";
        }

    }
}
