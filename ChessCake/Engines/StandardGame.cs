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

        public int Turn { get; private set; }

        public IDictionary<IPlayer, IList<BasePiece>> Pieces { get; private set; }

        public IDictionary<IPlayer, IList<BasePiece>> CapturedPieces { get; private set; }

        public bool InCheck { get; private set; }

        public bool IsCheckMate { get; private set; }

        public StandardGame(IDictionary<ChessColor, IPlayer> players) {
            Board = ChessFactory.CreateBoard();

            Players = players;
            Turn = 1;
            CurrentPlayer = FindPlayer(ChessColor.WHITE);

            Pieces = initPieces();
            CapturedPieces = initCapturedPieces();

            ValidateStandardGame();

            InitBoard();

            movementProvider = GameFactory.CreateMovementProvider(this);

        }
        
        public void Initialize() {
            while (!IsCheckMate) {
                try {

                    Common.ClearConsole();

                    Screen.PrintMatch(this);

                    IPosition source = InputProvider.ReadChessPosition().ToPosition();
                    IList<ICell> legalMoves = LegalMoves(source);

                    Common.ClearConsole();

                    Screen.PrintBoard(this, legalMoves);

                    IPosition target = InputProvider.ReadChessPosition(false).ToPosition();

                    IMovement nextMove = ChessFactory.CreateMovement(Board.GetCell(source), Board.GetCell(target), CurrentPlayer);

                    PerformMove(nextMove);

                } catch (ChessException e) {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
            }

            Common.ClearConsole();
            Screen.PrintMatch(this);

        }

        private void PerformMove(IMovement move) {
            ValidateSource(move.Source);
            ValidateTarget(move.Source, move.Target);

            move = MakeMove(move);

            movementProvider.Update(this);

            ValidateCheck(move);

            if(ValidateCheckMate(FindOpponentPlayer())) IsCheckMate = true;

            else NextTurn();
        }

        private IMovement MakeMove(IMovement move) {
            BasePiece movedPiece = Board.RemovePiece(move.Source.Position);
            movedPiece.IncreaseMoveCount();
            move.MovedPiece = movedPiece;

            BasePiece capturedPiece = Board.RemovePiece(move.Target.Position);
            move.CapturedPiece = capturedPiece;

            Board.PlacePiece(movedPiece, move.Target);

            if (!Common.IsObjectNull(capturedPiece)) {
                Pieces[move.Player].Remove(capturedPiece);
                CapturedPieces[CurrentPlayer].Add(capturedPiece);

            }

            return move;

        }

        private void NextTurn() {
            Turn++;
            CurrentPlayer = CurrentPlayer.Color == ChessColor.WHITE ? BlackPlayer : WhitePlayer;

        }

        private IMovement UndoMove(IMovement move) {
            BasePiece capturedPiece = move.CapturedPiece;

            BasePiece movedPiece = Board.RemovePiece(move.Target);
            movedPiece.DecreaseMoveCount();
            move.MovedPiece = movedPiece;

            Board.PlacePiece(movedPiece, move.Source);
            
            if(!Common.IsObjectNull(capturedPiece)) {
                Board.PlacePiece(capturedPiece, move.Target);
                Pieces[move.Player].Add(capturedPiece);
                CapturedPieces[move.Player].Remove(capturedPiece);
            }

            return move;

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

        public IPlayer FindOpponentPlayer() {
            return CurrentPlayer.Color == ChessColor.WHITE ? BlackPlayer : WhitePlayer;
        }

        public IPlayer FindOpponentPlayer(IPlayer player) {
            return player.Color == ChessColor.WHITE ? BlackPlayer : WhitePlayer;
        }

        private IList<BasePiece> FetchPieces(IPlayer player) {
            return Pieces[player];
        }

        private IList<BasePiece> FetchOpponentPieces(IPlayer player) {
            return Pieces[FindOpponentPlayer(player)];
        }

        private IList<BasePiece> FetchOpponentPieces() {
            return Pieces[FindOpponentPlayer()];
        }

        private ICell FindKing(ChessColor color) {
            foreach(ICell cell in Board.Grid) {
                BasePiece piece = cell.Piece;
                if(!Common.IsObjectNull(piece) && piece.Type == PieceType.KING && piece.Color == color) {
                    return cell;
                }
            }

            throw new IllegalStateException("There is no king on the board");
        }

        private void ValidateCheck(IMovement move) {
            if (IsPlayerInCheck(CurrentPlayer)) {
                UndoMove(move);
                throw new ChessException("You can't put yourself in check!");

            }

            InCheck = IsPlayerInCheck(FindOpponentPlayer()) ? true : false;

        }

        private bool ValidateCheckMate(IPlayer player) {
            if (!IsPlayerInCheck(player)) return false;

            IList<BasePiece> playerPieces = FetchPieces(player);
            movementProvider.UpdateCurrentPlayer(player);

            foreach(BasePiece piece in playerPieces) {
                IList<ICell> legalMoves = LegalMoves(piece.Position, false);

                foreach(ICell target in legalMoves) {
                    ICell source = Board.GetCell(piece.Position);
                    IMovement possibleMove = MakeMove(ChessFactory.CreateMovement(source, target, player));
                    bool inCheck = IsPlayerInCheck(player, true);

                    UndoMove(possibleMove);

                    if (!inCheck) return false;
                }
            }

            return true;

        }

        private bool IsPlayerInCheck(IPlayer player, bool isCheckmateValidation = false) {
            ICell kingCell = FindKing(player.Color);
            IList<BasePiece> opponentPieces = FetchOpponentPieces(player);

            if (isCheckmateValidation) movementProvider.UpdateCurrentPlayer(FindOpponentPlayer(player));

            foreach(BasePiece piece in opponentPieces) {
                IList<ICell> legalMoves = LegalMoves(piece.Position, false);
                if(legalMoves.Contains(kingCell)) {
                    return true;
                }
            }

            return false;
        }

        private void AddPieceOnPlayer(BasePiece piece) {
            IPlayer player = FindPlayer(piece.Color);
            Pieces[player].Add(piece);
        }

        private void PlaceNewPiece(BasePiece piece, ChessPosition chessPosition) {
            Board.PlacePiece(piece, Board.GetCell(chessPosition.ToPosition()));
            AddPieceOnPlayer(piece);
        }

        public bool IsThereOpponentPiece(ICell cell) {
            BasePiece piece = Board.FindPiece(cell.Position);
            return piece != null && piece.Color != CurrentPlayer.Color;
        }

        public IList<ICell> LegalMoves(IPosition sourcePosition, bool validateSource = true) {
            ICell sourceCell = Board.GetCell(sourcePosition);
            if(validateSource) ValidateSource(sourceCell);
            IList<ICell> legalMoves = movementProvider.GenerateLegalMoves(sourceCell);
            return legalMoves;
        }

        public IList<ICell> LegalMoves(ICell sourceCell) {
            return LegalMoves(sourceCell.Position);
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
                Pawn pawn = (Pawn)ChessFactory.CreatePiece(PieceType.PAWN, color, ChessFactory.CreatePosition(row, i));
                PlaceNewPiece(pawn, ChessFactory.CreateChessPosition((char)(i + 'A'), row));
            }
        }

        private void AddMajorPiecesOnBoard(ChessColor color, int row) {
            for (int i = 0; i < GlobalConstants.STANDARD_BOARD_ROWS; i++) {
                PieceType type = GameConstants.MAJOR_PIECES_SEQUENCE.ElementAt(i);
                BasePiece piece = ChessFactory.CreatePiece(type, color, ChessFactory.CreatePosition(row, i));
                PlaceNewPiece(piece, ChessFactory.CreateChessPosition((char)(i + 'A'), row));
            }
        }


        private void InitBoard() {
            Player firstPlayer = (Player)Players.Values.First();
            Player secondPlayer = (Player)Players.Values.ElementAt(1);

            BasePiece piece = ChessFactory.CreatePiece(PieceType.ROOK, secondPlayer.Color, ChessFactory.CreateChessPosition('h', 7).ToPosition());
            PlaceNewPiece(piece, ChessFactory.CreateChessPosition('h', 7));

            piece = ChessFactory.CreatePiece(PieceType.ROOK, secondPlayer.Color, ChessFactory.CreateChessPosition('d', 1).ToPosition());
            PlaceNewPiece(piece, ChessFactory.CreateChessPosition('d', 1));

            piece = ChessFactory.CreatePiece(PieceType.KING, secondPlayer.Color, ChessFactory.CreateChessPosition('e', 1).ToPosition());
            PlaceNewPiece(piece, ChessFactory.CreateChessPosition('e', 1));


            piece = ChessFactory.CreatePiece(PieceType.ROOK, firstPlayer.Color, ChessFactory.CreateChessPosition('b', 8).ToPosition());
            PlaceNewPiece(piece, ChessFactory.CreateChessPosition('b', 8));

            piece = ChessFactory.CreatePiece(PieceType.KING, firstPlayer.Color, ChessFactory.CreateChessPosition('a', 8).ToPosition());
            PlaceNewPiece(piece, ChessFactory.CreateChessPosition('a', 8));

            //AddMajorPiecesOnBoard(firstPlayer.Color, GameConstants.INITIAL_MAJOR_ROW_OF_FIRST_PLAYER);
            ////AddPawnsOnBoard(firstPlayer.Color, GameConstants.INITIAL_PAWNS_ROW_OF_FIRST_PLAYER);

            //AddMajorPiecesOnBoard(secondPlayer.Color, GameConstants.INITIAL_MAJOR_ROW_OF_SECOND_PLAYER);
            ////AddPawnsOnBoard(secondPlayer.Color, GameConstants.INITIAL_PAWNS_ROW_OF_SECOND_PLAYER);

        }

        private IDictionary<IPlayer, IList<BasePiece>> initCapturedPieces() {
            return new Dictionary<IPlayer, IList<BasePiece>>() {
                [WhitePlayer] = new List<BasePiece>(),
                [BlackPlayer] = new List<BasePiece>()
            };
        }

        private IDictionary<IPlayer, IList<BasePiece>> initPieces() {
            return new Dictionary<IPlayer, IList<BasePiece>>() {
                [WhitePlayer] = new List<BasePiece>(),
                [BlackPlayer] = new List<BasePiece>()
            };
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
