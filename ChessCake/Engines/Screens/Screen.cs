using ChessCake.Commons.Enumerations;
using ChessCake.Exceptions;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Boards.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ConsoleColor = System.Drawing.Color;
using FigletFont = Colorful.FigletFont;
using Figlet = Colorful.Figlet;
using Console = Colorful.Console;
using System;
using System.Collections.Generic;
using System.Drawing;
using ChessCake.Commons.Constants;
using ChessCake.Engines.Contracts;
using ChessCake.Models.Players.Contracts;
using System.Linq;

namespace ChessCake.Engines.Screens
{
    class Screen {

        public static void PrintHome() {
            Console.Clear();
            FigletFont font = FigletFont.Load("../../../Assets/Fonts/standard.flf");
            Figlet figlet = new Figlet(font);

            Console.WriteLine();
            PrintDivider(figlet);
            Console.WriteLine(figlet.ToAscii(GetTitle()), ColorTranslator.FromHtml("#FFD700"));
            PrintDivider(figlet);
        }

        private static String GetTitle() {
            return "  " + GlobalConstants.APP_NAME + "_";
        }

        public static void PrintWelcome() {
            const string firstMessage = "Hello, welcome!";
            const string secondMessage = "Do you want to start a game?";

            PrintMessage(firstMessage, secondMessage);
        }

        public static void PrintMessage(string msg1, string msg2) {
            Console.WriteLine(String.Format(GlobalConstants.FIRST_MESSAGE_ON_CHARACTER, msg1), Color.Gold);
            Console.WriteLine(String.Format(GlobalConstants.SECOND_MESSAGE_ON_CHARACTER, msg2), Color.Gold);
            PrintCharacter();

        }

        public static void PrintCharacter() {
            Console.WriteLine("    |||||", Color.White);
            Console.WriteLine("   ||O O|`____.", Color.White);
            Console.WriteLine("  |||\\-/|| \\ __\\", Color.White);
            Console.WriteLine("  |.--:--|  .   :", Color.White);
            Console.WriteLine(" (~m  : /  | oo:|", Color.White);
            Console.WriteLine(" ~~~~~~~~~~~~~~~~~%s\n\n", Color.Beige);
        }

        public static void PrintDivider(Figlet figlet) {
            Console.WriteLine(figlet.ToAscii(GlobalConstants.DEFAULT_ASTERISKS_DIVIDER), ColorTranslator.FromHtml("#8AFFEF"));

        }

        public static void PrintMatch(IEngine engine) {
            PrintCurrentPlayer(engine.CurrentPlayer);

            PrintTurn(engine.Turn);
            Console.WriteLine();

            PrintBoard(engine.Board);
            Console.WriteLine("\n");

            PrintCapturedPieces(engine.CapturedPieces);
            Console.WriteLine();

            HandleCheck(engine);

        }

        private static void HandleCheck(IEngine engine) {
            if (!(engine.Status == GameStatus.FINISHED)) {
                if (engine.Status == GameStatus.CHECK) {
                    PrintInCheck();
                }
            } else {
                PrintCheckMate(engine.CurrentPlayer);
            }
        }

        private static void PrintInCheck() {
            PrintMessage("Pay Attention!", "Match in Check!");
        }

        private static void PrintCheckMate(IPlayer player) {
            PrintMessage("CheckMate !!", "Congratulations to Player " + player.Name + "!");
        }

        public static void PrintCurrentPlayer(IPlayer currentPlayer) {
            Console.WriteLine(String.Format(GlobalConstants.CURRENT_PLAYER_FORMATTER, currentPlayer.Name, currentPlayer.Color));
        }

        public static void PrintTurn(int turn) {
            Console.WriteLine("Turn Count: " + turn);

        }

        public static void PrintBoard(IBoard board) {
            for (int i = 0; i < board.Rows; i++)
            {
                Console.Write("(" + (8 - i) + ") ");
                for (int j = 0; j < board.Columns; j++)
                {
                    Console.Write("║");
                    PrintPiece(board.FindPiece(i, j));
                    Console.Write("║");

                }
                Console.WriteLine();
            }
            Console.Write("    (A)(B)(C)(D)(E)(F)(G)(H)");
        }

        public static void PrintBoard(IEngine engine, IList<ICell> possibleMoves) {
            IBoard board = engine.Board;
            IPlayer currentPlayer = engine.CurrentPlayer;

            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkGray;

            Console.WriteLine(String.Format(GlobalConstants.CURRENT_PLAYER_FORMATTER, currentPlayer.Name, currentPlayer.Color));

            for (int i = 0; i < board.Rows; i++)
            {
                Console.Write("(" + (8 - i) + ") ");
                for (int j = 0; j < board.Columns; j++)
                {
                    Console.Write("║");
                    if (possibleMoves.Contains(board.GetCell(i, j)))
                    {
                        Console.BackgroundColor = fundoAlterado;
                    }
                    else
                    {
                        Console.BackgroundColor = fundoOriginal;
                    }
                    PrintPiece(board.FindPiece(i, j));
                    Console.BackgroundColor = fundoOriginal;
                    Console.Write("║");

                }
                Console.WriteLine();
            }
            Console.Write("    (A)(B)(C)(D)(E)(F)(G)(H)");
        }

        public static void PrintSourceInput() {
            Console.Write("Source: ");
        }

        public static void PrintTargetInput() {
            Console.WriteLine("\n");
            Console.Write("Target: ");
        }

        public static void PrintPiece(BasePiece piece)
        {
            if (piece == null)
            {
                Console.Write("-");
            }
            else
            {
                if (piece.Color == ChessColor.WHITE)
                {
                    Console.Write(piece);
                }
                else
                {
                    Console.Write(piece, ConsoleColor.Yellow);
                }
            }

        }

        private static void PrintCapturedPieces(IDictionary<IPlayer, IList<BasePiece>> capturedPieces) {
            IPlayer whitePlayer = capturedPieces.FirstOrDefault(x => x.Key.Color == ChessColor.WHITE).Key;
            IPlayer blackPlayer = capturedPieces.FirstOrDefault(x => x.Key.Color == ChessColor.BLACK).Key;

            Console.WriteLine("Captured Pieces By Player: ");
            Console.Write(whitePlayer.Name + ": ");
            Console.WriteLine(string.Join(", ", capturedPieces[whitePlayer]), Color.Yellow);

            Console.Write(blackPlayer.Name + ": ");
            Console.WriteLine(string.Join(", ", capturedPieces[blackPlayer]));

        }

        public static void PrintPromotedPiece() {
            Console.WriteLine();
            Console.WriteLine("Pieces available for promotion:");
            Console.WriteLine("- [Q]: Queen");
            Console.WriteLine("- [R]: Rook");
            Console.WriteLine("- [B]: Bishop");
            Console.WriteLine("- [N]: Knight");
            Console.WriteLine();
            Console.Write("Choose the piece you want to promote: ");
        }
    }
}
