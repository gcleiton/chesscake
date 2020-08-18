using ChessCake.Engines.Contracts;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Players.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Providers.Movements.Contracts {
    public interface IMovementProvider {

        public IEngine Engine { get; }

        public IList<ICell> GenerateLegalMoves(ICell source);

        public bool IsLegalMovement(ICell source, ICell target);

        public bool IsThereAnyLegalMove(ICell source);


        public void Update(IEngine engine);

        public void UpdateCurrentPlayer(IPlayer player);

    }
}
