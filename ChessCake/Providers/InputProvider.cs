using ChessCake.Commons;
using ChessCake.Commons.Enumerations;
using ChessCake.Models.Players.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Providers
{
    class InputProvider {

        public static IDictionary<ChessColor, IPlayer> ReadPlayers() {
            var players = new Dictionary<ChessColor, IPlayer>();
            StringBuilder playerNameFormatter = new StringBuilder("Qual é o nome do {0}º jogador?");
            IList<ChessColor> orderPlayers = Common.GenerateOrderPlayers();

            for (int i = 0; i < GlobalConstants.STANDARD_NUMBER_OF_PLAYERS_GAME; i++) {
                Console.Write(string.Format(playerNameFormatter.ToString(), i));

                string name = Console.ReadLine();
                ChessColor playerColor = orderPlayers[i];
                IPlayer player = ChessFactory.CreatePlayer(name, playerColor);

                players.Add(playerColor, player);

            }

            return players;

        }



    }
}
