﻿using ChessCake.Commons;
using ChessCake.Commons.Constants;
using ChessCake.Commons.Enumerations;
using ChessCake.Engines.Contracts;
using ChessCake.Engines.Screens;
using ChessCake.Exceptions;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Boards.Contracts;
using ChessCake.Models.ChessPositions.Contracts;
using ChessCake.Models.Movements.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Players.Contracts;
using ChessCake.Models.Positions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ChessCake.Providers.Inputs
{
    class InputProvider {

        public static Boolean ReadStartGame() {
            Screen.PrintHome(); // problema de cor de Fundo aqui
            Screen.PrintWelcome();
            Console.WriteLine();
            Console.Write("Type 'yes' to start the game, otherwise type 'no' to quit application: ");
            string answer = Console.ReadLine().ToUpper();

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

            for (int i = 0; i < GameConstants.STANDARD_NUMBER_OF_PLAYERS_GAME; i++) {
                Console.Write(string.Format(ScreenConstants.PLAYER_NAME_FORMATTER_INPUT, i + 1));

                string name = Console.ReadLine();
                ChessColor playerColor = orderPlayers[i];
                IPlayer player = ChessFactory.CreatePlayer(name, playerColor);

                players.Add(playerColor, player);

            }

            return players;

        }

        public static PieceType ReadPromotedPiece() {

            Screen.PrintPromotedPiece();

            char typeChar = Console.ReadKey().KeyChar;

            PieceType type = (PieceType) typeChar;

            if (GameConstants.AVAILABLE_PIECES_FOR_PROMOTION.Contains(type)) return type;

            throw new ChessException("Error reading character of the piece to be promoted! Try again.");

        }

        public static IChessPosition ReadChessPosition(bool isSource = true) {
            try {
                if (isSource) Screen.PrintSourceInput();
                else Screen.PrintTargetInput();
               
                string pos = Console.ReadLine();
                string pattern = @"([A-Ha-h])([1-9])";

                Regex regex = new Regex(pattern);
                Match match = regex.Match(pos);

                char column = char.Parse(match.Groups[1].ToString().ToUpper());
                int row = int.Parse(match.Groups[2].ToString());

                return ChessFactory.CreateChessPosition(column, row);

            } catch (Exception e) {
                throw new ChessException("Error reading chess position! Try again.");
            }
        }

    }

    class CopyOfInputProvider {

        public static Boolean ReadStartGame() {
            Screen.PrintHome(); // problema de cor de Fundo aqui
            Screen.PrintWelcome();
            Console.WriteLine();
            Console.Write("Type 'yes' to start the game, otherwise type 'no' to quit application: ");
            string answer = Console.ReadLine().ToUpper();

            switch (answer) {
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

            for (int i = 0; i < GameConstants.STANDARD_NUMBER_OF_PLAYERS_GAME; i++) {
                Console.Write(string.Format(ScreenConstants.PLAYER_NAME_FORMATTER_INPUT, i + 1));

                string name = Console.ReadLine();
                ChessColor playerColor = orderPlayers[i];
                IPlayer player = ChessFactory.CreatePlayer(name, playerColor);

                players.Add(playerColor, player);

            }

            return players;

        }

        public static PieceType ReadPromotedPiece() {

            Screen.PrintPromotedPiece();

            char typeChar = Console.ReadKey().KeyChar;

            PieceType type = (PieceType)typeChar;

            if (GameConstants.AVAILABLE_PIECES_FOR_PROMOTION.Contains(type)) return type;

            throw new ChessException("Error reading character of the piece to be promoted! Try again.");

        }

        public static IChessPosition ReadChessPosition(bool isSource = true) {
            try {
                if (isSource) Screen.PrintSourceInput();
                else Screen.PrintTargetInput();

                string pos = Console.ReadLine();
                string pattern = @"([A-Ha-h])([1-9])";

                Regex regex = new Regex(pattern);
                Match match = regex.Match(pos);

                char column = char.Parse(match.Groups[1].ToString().ToUpper());
                int row = int.Parse(match.Groups[2].ToString());

                return ChessFactory.CreateChessPosition(column, row);

            } catch (Exception e) {
                throw new ChessException("Error reading chess position! Try again.");
            }
        }

    }
}
