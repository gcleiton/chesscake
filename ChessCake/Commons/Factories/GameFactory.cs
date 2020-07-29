using ChessCake.Commons.Enumerations;
using ChessCake.Engines;
using ChessCake.Engines.Contracts;
using ChessCake.Models.Players.Contracts;
using ChessCake.Providers.Inputs;
using ChessCake.Providers.Movements;
using ChessCake.Providers.Movements.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Commons {
    class GameFactory {
            
        public static StandardGame CreateStandardGame(IDictionary<ChessColor, IPlayer> players) {
            return new StandardGame(players);
        }

        public static IMovementProvider CreateMovementProvider(IEngine engine) {
            return new MovementProvider(engine);
        }

    }
}
