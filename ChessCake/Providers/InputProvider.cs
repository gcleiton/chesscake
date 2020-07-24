using ChessCake.Commons;
using ChessCake.Commons.Enumerations;
using ChessCake.Engines.Screens;
using ChessCake.Models.Players.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Providers
{
    class InputProvider {

        public static Boolean ReadStartGame() {
            Screen.PrintHome();
            Screen.PrintWelcome();
            Console.WriteLine();
            Console.Write("Type 'yes' to start the game, otherwise type 'no' to quit application: ");
            string answer = Console.ReadLine().ToUpper();
            Console.WriteLine(answer); 

            switch(answer) {
                case "YES":
                    return true;

                case "NO": 
                    Common.QuitGame();
                    break;

                default:
                    Console.WriteLine("Invalid choice, please try again...");
                    ReadStartGame();
                    break;

            }

            return true;

        }

        public static IDictionary<ChessColor, IPlayer> ReadPlayers() {
            Console.Clear();
            Screen.PrintHome();

            const string firstMessage = "Alright, before you start,";
            const string secondMessage = "choose the names of the players, below:";

            Screen.PrintMessage(firstMessage, secondMessage);

            var players = new Dictionary<ChessColor, IPlayer>();
            IList<ChessColor> orderPlayers = Common.GenerateOrderPlayers();

            for (int i = 0; i < GlobalConstants.STANDARD_NUMBER_OF_PLAYERS_GAME; i++) {
                Console.Write(string.Format(GlobalConstants.PLAYER_NAME_FORMATTER_INPUT, i + 1));

                string name = Console.ReadLine();
                ChessColor playerColor = orderPlayers[i];
                IPlayer player = ChessFactory.CreatePlayer(name, playerColor);

                players.Add(playerColor, player);

            }

            return players;

        }



    }
}
