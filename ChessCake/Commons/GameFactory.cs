using ChessCake.Commons.Enumerations;
using ChessCake.Engines;
using ChessCake.Models.Players.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Commons {
    class GameFactory {
            
        public static StandardGame CreateStandardGame(IDictionary<ChessColor, IPlayer> players) {
            return new StandardGame(players);
        }

    }
}
