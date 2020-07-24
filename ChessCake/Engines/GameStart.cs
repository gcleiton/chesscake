using ChessCake.Commons;
using ChessCake.Commons.Enumerations;
using ChessCake.Engines.Screens;
using ChessCake.Models.Players.Contracts;
using ChessCake.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Engines
{
    public class GameStart {

        public static void Start() {
            try {
                bool canStartGame = InputProvider.ReadStartGame();

                Console.WriteLine(canStartGame);

                if (!canStartGame) Common.QuitGame();

                IDictionary<ChessColor, IPlayer> players = InputProvider.ReadPlayers();

                StandardGame engine = GameFactory.CreateStandardGame(players);

                engine.Initialize();

            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

        }

    }
}
