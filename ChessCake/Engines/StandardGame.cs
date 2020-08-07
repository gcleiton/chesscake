using ChessCake.Commons;
using ChessCake.Commons.Constants;
using ChessCake.Commons.Enumerations;
using ChessCake.Engines.Contracts;
using ChessCake.Engines.Screens;
using ChessCake.Exceptions;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Boards.Contracts;
using ChessCake.Models.Movements.Contracts;
using ChessCake.Models.Pieces;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Players;
using ChessCake.Models.Players.Contracts;
using ChessCake.Models.Positions.Chess;
using ChessCake.Models.Positions.Contracts;
using ChessCake.Providers.Inputs;
using ChessCake.Providers.Movements.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessCake.Engines {
    class StandardGame : IEngine {
        public IBoard Board { get; private set; }

        private IMovementProvider movementProvider;

        private readonly IDictionary<ChessColor, IPlayer> Players; // use OrderedDictionary

        public IPlayer CurrentPlayer { get; set; }

        public IPlayer BlackPlayer {
            get { 
                return Players[ChessColor.BLACK];
            } 
        }

        public IPlayer WhitePlayer {
            get {
                return Players[ChessColor.WHITE];
            }
        }

        private int Turn { get; set; }

        private IDictionary<ChessColor, IList<BasePiece>> CapturedPieces;

        public StandardGame(IDictionary<ChessColor, IPlayer> players) {
            Board = ChessFactory.CreateBoard();

            this.Players = players;
            this.CapturedPieces = new Dictionary<ChessColor, IList<BasePiece>>();
            this.Turn = 1;
            this.CurrentPlayer = FindPlayer(ChessColor.WHITE);

            this.ValidateStandardGame();

            InitBoard();

            movementProvider = GameFactory.CreateMovementProvider(this);

        }

        public void Initialize() {
            while (true) {
                try {

                    Common.ClearConsole();

                    Screen.PrintBoard(this);

                    IPosition source = InputProvider.ReadChessPosition().ToPosition();
                    IList<ICell> legalMoves = LegalMoves(source);

                    Common.ClearConsole();

                    Screen.PrintBoard(this, legalMoves);

                    IPosition target = InputProvider.ReadChessPosition(false).ToPosition();

                    IMovement nextMove = ChessFactory.CreateMovement(Board.GetCell(source), Board.GetCell(target));

                    PerformMove(nextMove);

                } catch (ChessException e) {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
            }
        }

        public void PerformMove(IMovement move) {
            ValidateSource(move.Source);
            ValidateTarget(move.Source, move.Target);

            MakeMove(move);

            movementProvider.Update(this);

            NextTurn();

        }

        public void NextTurn() {
            Turn++;
            CurrentPlayer = CurrentPlayer.Color == ChessColor.WHITE ? BlackPlayer : WhitePlayer;
     
        }

        public void MakeMove(IMovement move) {
            BasePiece movedPiece = Board.RemovePiece(move.Source.Position);
            movedPiece.increaseMoveCount();

            BasePiece capturedPiece = Board.RemovePiece(move.Target.Position);

            Board.PlacePiece(movedPiece, move.Target);

            if (!Common.IsObjectNull(capturedPiece)) {
                Console.WriteLine(capturedPiece);
                CapturedPieces[CurrentPlayer.Color].Add(capturedPiece);

            }

        }

        private void ValidateStandardGame() {
            if (Players.Count != GlobalConstants.STANDARD_NUMBER_OF_PLAYERS_GAME) {
                throw new ChessException("Standard game needs two human players to start.");
            }
            if (Board.Rows != GlobalConstants.STANDARD_BOARD_ROWS || Board.Columns != GlobalConstants.STANDARD_BOARD_COLUMNS) {
                throw new BoardException("Standard game needs a 8x8 board.");
            }
        }

        public IPlayer FindPlayer(ChessColor color) {
            return Players[color];
        }

        private void AddPieceOnPlayer(ChessColor color, BasePiece piece) {
            Players[color].AddPiece(piece);
        }

        private void PlaceNewPiece(BasePiece piece, ChessPosition chessPosition) {
            Board.PlacePiece(piece, Board.GetCell(chessPosition.ToPosition()));
            //AddPieceOnPlayer(piece.Color, piece);

        }

        public bool IsThereOpponentPiece(ICell cell) {
            BasePiece piece = Board.FindPiece(cell.Position);
            return piece != null && piece.Color != CurrentPlayer.Color;
        }

        public IList<ICell> LegalMoves(IPosition sourcePosition) {
            ICell sourceCell = Board.GetCell(sourcePosition);
            ValidateSource(sourceCell);
            var test = movementProvider.GenerateLegalMoves(sourceCell);
            return test;
        }

        private void ValidateSource(ICell source) {
            if (!source.IsOccupied()) {
                throw new ChessException("Não existe nenhuma peça na posição de origem informado.");
            }
            if (CurrentPlayer != FindPlayer(source.Piece.Color)) {
                throw new ChessException("A peça selecionada não pertence a você.");
            }
            if (!movementProvider.IsThereAnyLegalMove(source)) {
                throw new ChessException("Não existe movimentos possíveis para a peça selecionada.");
            }
        }

        private void ValidateTarget(ICell source, ICell target) {
            if (!movementProvider.IsLegalMovement(source, target)) {
                throw new ChessException("A peça selecionada não pode mover para essa posição.");
            }
        }

        private void AddPawnsOnBoard(ChessColor color, int row) {
            for (int i = 0; i < GlobalConstants.STANDARD_BOARD_ROWS; i++) {
                Pawn pawn = (Pawn)ChessFactory.CreatePiece(PieceType.PAWN, color);
                PlaceNewPiece(pawn, ChessFactory.CreateChessPosition((char)(i + 'A'), row));
            }
        }

        private void AddMajorPiecesOnBoard(ChessColor color, int row) {
            for (int i = 0; i < GlobalConstants.STANDARD_BOARD_ROWS; i++) {
                PieceType type = GameConstants.MAJOR_PIECES_SEQUENCE.ElementAt(i);
                BasePiece piece = ChessFactory.CreatePiece(type, color);
                PlaceNewPiece(piece, ChessFactory.CreateChessPosition((char)(i + 'A'), row));
            }
        }


        private void InitBoard() {
            Player firstPlayer = (Player)Players.Values.First();
            Player secondPlayer = (Player)Players.Values.ElementAt(1);

            AddMajorPiecesOnBoard(firstPlayer.Color, GameConstants.INITIAL_MAJOR_ROW_OF_FIRST_PLAYER);
            AddPawnsOnBoard(firstPlayer.Color, GameConstants.INITIAL_PAWNS_ROW_OF_FIRST_PLAYER);

            AddMajorPiecesOnBoard(secondPlayer.Color, GameConstants.INITIAL_MAJOR_ROW_OF_SECOND_PLAYER);
            AddPawnsOnBoard(secondPlayer.Color, GameConstants.INITIAL_PAWNS_ROW_OF_SECOND_PLAYER);

        }


        //    public Player findPlayer(Color color) {
        //        return players[color];
        //    }

        //    public bool isThereOpponentPiece(Cell cell) {
        //        Piece piece = board.findPiece(cell.position);
        //        return piece != null && piece.color != currentPlayer.color;
        //    }

        //    public bool isThereAPiece(Cell cell) {
        //        Piece piece = board.findPiece(cell.position);
        //        return piece != null;
        //    }

        //    public void nextTurn() {
        //        turn++;
        //        currentPlayer = currentPlayer == whitePlayer ? blackPlayer : whitePlayer;
        //    }

        //    private void updatePlayers(Player whitePlayer, Player blackPlayer) {
        //        players[Color.WHITE] = whitePlayer;
        //        players[Color.BLACK] = blackPlayer;
        //    }

    }
}
