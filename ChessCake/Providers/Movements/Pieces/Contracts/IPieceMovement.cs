using ChessCake.Engines.Contracts;
using ChessCake.Models.Boards.Cells.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Providers.Movements.Pieces.Contracts {
    public interface IPieceMovement {

        public IList<ICell> GenerateLegalMoves();

    }

}