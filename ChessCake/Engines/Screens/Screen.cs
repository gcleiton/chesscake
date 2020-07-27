using ChessCake.Commons.Enumerations;
using ChessCake.Exceptions;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Boards.Contracts;
using ChessCake.Models.ChessPositions.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Positions.Chess;
using ConsoleColor = System.Drawing.Color;
using FigletFont = Colorful.FigletFont;
using Figlet = Colorful.Figlet;
using Console = Colorful.Console;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Drawing;
using ChessCake.Commons;

namespace ChessCake.Engines.Screens
{
    class Screen {

        public static void PrintHome() {
            Console.Clear();
            FigletFont font = FigletFont.Load("../../../Assets/Fonts/standard.flf");
            Figlet figlet = new Figlet(font);

            Console.WriteLine();
            PrintDivider(figlet);
            Console.WriteLine(figlet.ToAscii(GlobalConstants.DEFAULT_ASTERISKS_DIVIDER), ColorTranslator.FromHtml("#8AFFEF"));
            PrintDivider(figlet);
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

        public static void PrintBoard(IBoard board)
        {
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

        public static void PrintBoard(IBoard board, List<ICell> possibleMoves)
        {

            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkGray;

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
            Console.WriteLine('\n');
            Console.Write("Source: ");
        }

        public static void PrintTargetInput() {
            Console.WriteLine();
            Console.Write("Target: ");
        }

        public static IChessPosition ReadChessPosition(bool isSource = true)
        {
            try
            {
                if (isSource)
                {
                    Console.Write("Source: ");
                }
                else
                {
                    Console.Write("Target: ");
                }
                string pos = Console.ReadLine();
                string pattern = @"([A-Za-z])([1-9])";

                Regex regex = new Regex(pattern);
                Match match = regex.Match(pos);

                char column = char.Parse(match.Groups[1].ToString().ToUpper());
                int row = int.Parse(match.Groups[2].ToString());

                return new ChessPosition(column, row);

            }
            catch (Exception e)
            {
                throw new ChessException("Erro ao ler a posição do xadrez. Tente novamente.");
            }
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
    }
}
