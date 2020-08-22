using ChessCake.Commons;
using ChessCake.Commons.Enumerations;
using ChessCake.Engines.Screens;
using ChessCake.Models.Players;
using ChessCake.Models.Players.Contracts;
using ChessCake.Providers;
using ChessCake.Providers.Inputs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Engines
{
    public class GameStart {

        public static void Start() {
            //try {
                //bool canStartGame = InputProvider.ReadStartGame();

                //Console.WriteLine(canStartGame);

                //if (!canStartGame) Common.QuitGame();

                //IDictionary<ChessColor, IPlayer> players = InputProvider.ReadPlayers();

                IDictionary<ChessColor, IPlayer> players = new Dictionary<ChessColor, IPlayer>()
                {
                    { ChessColor.BLACK, ChessFactory.CreatePlayer("Gabriel", ChessColor.BLACK)},
                    { ChessColor.WHITE, ChessFactory.CreatePlayer("Cleiton", ChessColor.WHITE)}
                };

                StandardGame engine = GameFactory.CreateStandardGame(players);

                engine.Initialize();

            //} catch (Exception e) {
            //    Console.WriteLine(e.Message);
            //}

        }

    }
}
