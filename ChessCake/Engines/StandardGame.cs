﻿using ChessCake.Commons;
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

        public IPlayer CurrentPlayer { get; set; }

        public int Turn { get; private set; }

        public GameStatus Status { get; private set; }

        public BasePiece EnPassant { get; private set; }

        public IDictionary<IPlayer, IList<BasePiece>> Pieces { get; private set; }

        public IDictionary<IPlayer, IList<BasePiece>> CapturedPieces { get; private set; }

        public IDictionary<ChessColor, IPlayer> Players { get; private set; }

        public BasePiece Promoted { get; private set; }

        // Helper properties

        public IPlayer WhitePlayer {
            get {
                return Players[ChessColor.WHITE];
            }

            private set {
                if(value.Color == ChessColor.WHITE) {
                    WhitePlayer = value;
                }
            }
        }

        public IPlayer BlackPlayer {
            get {
                return Players[ChessColor.BLACK];
            }

            private set {
                if (value.Color == ChessColor.BLACK) {
                    BlackPlayer = value;
                }
            }

        }

        // Provider properties
        public IMovementProvider MovementProvider { get; private set; }

        public StandardGame(IDictionary<ChessColor, IPlayer> players) {
            Board = ChessFactory.CreateBoard(); 

            Players = players;
            Turn = 1;
            CurrentPlayer = FindPlayer(Common.GenerateOrderPlayers().ElementAt(0)); // randomly set the current player

            Pieces = initPieces();
            CapturedPieces = initCapturedPieces();

            ValidateStandardGame();

            InitBoard(); // initializes the pieces on the board

            MovementProvider = GameFactory.CreateMovementProvider(this);

        }

        public void Initialize() { // method to initialize engine match
            Status = GameStatus.RUNNING;

            while (Status != GameStatus.CHECKMATE) { // if InCheckmate == false end game

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

                    HandlePromotedPiece();

                } catch (ChessException e) {
                    HandleException(e);
                }
            }

            Status = GameStatus.FINISHED;

            Common.ClearConsole();
            Screen.PrintMatch(this);

        }

        private void HandleException(ChessException e) {
            Console.WriteLine(e.Message);
            Console.WriteLine("\nEnter any key to continue ...");
            Console.ReadKey();
        }

        private void NextTurn() {
            Turn++;
            CurrentPlayer = CurrentPlayer.Color == ChessColor.WHITE ? BlackPlayer : WhitePlayer;

        }

        // Movement methods
        private void PerformMove(IMovement move) {
            ICell source = move.Source;
            ICell target = move.Target;

            ValidateSource(source);
            ValidateTarget(source, target);

            move = MakeMove(move);

            if (IsPlayerInCheck(CurrentPlayer)) {
                
                UndoMove(move);
                throw new ChessException("You can't put yourself in check");

            }

            Promoted = null;
            if (move.MovedPiece.Type == PieceType.PAWN) {
                if (move.MovedPiece.Color == ChessColor.WHITE && move.Target.Position.Row == 0 ||
                    move.MovedPiece.Color == ChessColor.BLACK && move.Target.Position.Row == 7) {

                    Promoted = Board.FindPiece(move.Target);
                    Promoted = ReplacePromotedPiece(PieceType.QUEEN);
                }
            }

            ValidateCheck(move);

            Console.WriteLine("Checkmate: " + ValidateCheckMate(FindOpponentPlayer()));

            if (ValidateCheckMate(FindOpponentPlayer())) Status = GameStatus.CHECKMATE; // problema do rei sendo capturado
            else NextTurn();

            // Special move - En Passant

            if (IsEnPassantVulnerable(move)) EnPassant = move.MovedPiece;

            else EnPassant = null;

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

            // Special Move - Castling:

            if (IsCastlingMove(move)) {
                HandleCastlingMove(move);
            }

            // Special Move - EnPassant:

            if (CheckEnPassant(move)) {
                HandleEnPassantMove(move);

            }

            return move;

        }

        private IMovement UndoMove(IMovement move) {
            BasePiece capturedPiece = move.CapturedPiece;

            BasePiece movedPiece = Board.RemovePiece(move.Target);
            movedPiece.DecreaseMoveCount();
            move.MovedPiece = movedPiece;
            move.CapturedPiece = capturedPiece;

            Board.PlacePiece(movedPiece, move.Source);

            if (!Common.IsObjectNull(capturedPiece)) {
                Board.PlacePiece(capturedPiece, move.Target);
                Pieces[move.Player].Add(capturedPiece);
                CapturedPieces[FindOpponentPlayer(move.Player)].Remove(capturedPiece);
            }

            if (IsCastlingMove(move)) {
                HandleCastlingMove(move, true);
            }

            if (CheckEnPassant(move)) {
                HandleEnPassantMove(move, true);
            }

            return move;

        }


        // Special movement methods
        private IMovement MakeEnPassantMove(IMovement move) {
            BasePiece movedPiece = move.MovedPiece;
            BasePiece capturedPiece = move.CapturedPiece;

            if (move.Source.Position.Column != move.Target.Position.Column && Common.IsObjectNull(capturedPiece)) {

                ICell pieceToBeCaptured;

                if (movedPiece.Color == ChessColor.WHITE) {
                    pieceToBeCaptured = Board.FindNeighbor(move.Target, 1, GridCoordinate.ROW);

                } else {
                    pieceToBeCaptured = Board.FindNeighbor(move.Target, -1, GridCoordinate.ROW);
                }

                move.CapturedPiece = pieceToBeCaptured.Piece;

                capturedPiece = Board.RemovePiece(pieceToBeCaptured.Position);
                Pieces[move.Player].Remove(capturedPiece);
                CapturedPieces[CurrentPlayer].Add(capturedPiece);

            }

            return move;
        }

        private IMovement UndoEnPassantMove(IMovement move) {

            BasePiece movedPiece = move.MovedPiece;
            BasePiece capturedPiece = move.CapturedPiece;

            if (move.Source.Position.Column != move.Target.Position.Column && capturedPiece == EnPassant) {

                BasePiece pawnCaptured = Board.RemovePiece(move.Target.Position);
                ICell pawnCell;

                if (movedPiece.Color == ChessColor.WHITE) {
                    pawnCell = Board.FindNeighbor(move.Target, 1, GridCoordinate.ROW);

                } else {
                    pawnCell = Board.FindNeighbor(move.Target, -1, GridCoordinate.ROW);
                }

                Board.PlacePiece(pawnCaptured, pawnCell);

                Pieces[move.Player].Add(capturedPiece);
                CapturedPieces[FindOpponentPlayer(move.Player)].Remove(capturedPiece);

            }

            return move;
        }

        private IMovement MakeCastlingMove(CastlingDirection direction, IMovement kingMove) { // Right Rook
            IMovement move = GenerateCastlingMove(direction, kingMove);

            BasePiece capturedPiece = Board.RemovePiece(move.Source.Position);
            Board.PlacePiece(move.MovedPiece, move.Target);

            move.CapturedPiece = capturedPiece;
            move.MovedPiece.IncreaseMoveCount();

            return move;

        }

        private IMovement UndoCastlingMove(CastlingDirection direction, IMovement kingMove) {
            IMovement move = GenerateCastlingMove(direction, kingMove);

            Board.PlacePiece(move.MovedPiece, move.Source);
            move.MovedPiece.DecreaseMoveCount();

            return move;
        }


        // Handling special movements methods
        private IMovement HandleCastlingMove(IMovement kingMove, bool IsUndo = false) {
            if (IsRightCastlingMove(kingMove)) {
                if (IsUndo) return UndoCastlingMove(CastlingDirection.RIGHT, kingMove);
                return MakeCastlingMove(CastlingDirection.RIGHT, kingMove);
            }

            if (IsUndo) return UndoCastlingMove(CastlingDirection.LEFT, kingMove);

            return MakeCastlingMove(CastlingDirection.LEFT, kingMove);

        }

        private IMovement HandleEnPassantMove(IMovement move, bool isUndo = false) {
            if (!isUndo) return MakeEnPassantMove(move);

            return UndoEnPassantMove(move);

        }

        private void HandlePromotedPiece() {
            if (!Common.IsObjectNull(Promoted)) {
                PieceType type = InputProvider.ReadPromotedPiece();
                ReplacePromotedPiece(type);
            }

        }

        // Validation methods
        private void ValidateStandardGame() {
            if (Players.Count != GameConstants.STANDARD_NUMBER_OF_PLAYERS_GAME) {
                throw new ChessException("Standard game needs two human players to start.");
            }
            if (Board.Rows != BoardConstants.STANDARD_BOARD_ROWS || Board.Columns != BoardConstants.STANDARD_BOARD_COLUMNS) {
                throw new ChessException("Standard game needs a 8x8 board.");
            }
        }

        private void ValidateSource(ICell source) { // validate source move

            if (!source.IsOccupied()) {
                throw new ChessException("There is no piece in the originating position informed.");
            }

            if (CurrentPlayer != FindPlayer(source.Piece.Color)) {
                throw new ChessException("The selected piece does not belong to you.");
            }

            if (!MovementProvider.IsThereAnyLegalMove(source)) {
                throw new ChessException("There are no possible movements for the selected piece.");
            }

        }

        private void ValidateTarget(ICell source, ICell target) { // validate target move
            if (!MovementProvider.IsLegalMovement(source, target)) {
                throw new ChessException("The selected piece cannot move to that position.");
            }
        }

        private void ValidateCheck(IMovement move) { // check if the player's movement puts himself in check
            if (IsPlayerInCheck(CurrentPlayer)) {
                UndoMove(move);
                throw new ChessException("You can't put yourself in check!");

            }

            Status = IsPlayerInCheck(FindOpponentPlayer()) ? GameStatus.CHECK : GameStatus.RUNNING;

        }

        private bool IsPlayerInCheck(IPlayer player) {
            ICell kingCell = FindKing(player.Color);
            IList<BasePiece> opponentPieces = FetchOpponentPieces(player);

            foreach (BasePiece piece in opponentPieces) {
               
                IList<ICell> legalMoves = LegalMoves(piece.Position, true);
                
                if (legalMoves.Contains(kingCell)) {
                    return true;
                }
            }

            return false;
        }

        private bool ValidateCheckMate(IPlayer player) {
            if (!IsPlayerInCheck(player)) {
                return false;
            }

            IList<BasePiece> playerPieces = FetchPieces(player);

            foreach (BasePiece piece in playerPieces.ToList()) {
                IList<ICell> legalMoves = LegalMoves(piece.Position, true);

                foreach (ICell target in legalMoves) {
                    ICell source = Board.GetCell(piece.Position);
                    IMovement possibleMove = MakeMove(ChessFactory.CreateMovement(source, target, player));
                    bool inCheck = IsPlayerInCheck(player);

                    UndoMove(possibleMove);

                    if (!inCheck) return false;

                }
            }

            return true;

        }

        // Legal movements generator methods

        private IList<ICell> LegalMoves(IPosition sourcePosition, bool isTestCheck = false) { // generate possible moves of a piece

            ICell sourceCell = Board.GetCell(sourcePosition);

            if (!isTestCheck) ValidateSource(sourceCell);

            IList<ICell> legalMoves = MovementProvider.GenerateLegalMoves(sourceCell);

            return legalMoves;
        }

        public IList<ICell> LegalMoves(ICell sourceCell) {
            return LegalMoves(sourceCell.Position);
        }

        private IMovement GenerateCastlingMove(CastlingDirection direction, IMovement kingMove) {
            ICell source, target;

            if (direction == CastlingDirection.RIGHT) {
                source = Board.GetCell(kingMove.Source.Position.Row, kingMove.Source.Position.Column + 3);
                target = Board.GetCell(kingMove.Source.Position.Row, kingMove.Source.Position.Column + 1);
                return ChessFactory.CreateMovement(source, target, CurrentPlayer);

            }

            source = Board.GetCell(kingMove.Source.Position.Row, kingMove.Source.Position.Column - 4);
            target = Board.GetCell(kingMove.Source.Position.Row, kingMove.Source.Position.Column - 1);

            return ChessFactory.CreateMovement(source, target, CurrentPlayer);

        }


        // Inicialization methods
        private IDictionary<IPlayer, IList<BasePiece>> initPieces() {
            return new Dictionary<IPlayer, IList<BasePiece>>() {
                [WhitePlayer] = new List<BasePiece>(),
                [BlackPlayer] = new List<BasePiece>()
            };
        }

        private IDictionary<IPlayer, IList<BasePiece>> initCapturedPieces() {
            return initPieces();
        }


        // Board initialization methods
        private void InitBoard() { // method to allocate pieces on the board
            Player firstPlayer = (Player)Players.Values.First();
            Player secondPlayer = (Player)Players.Values.ElementAt(1);

            AddMajorPiecesOnBoard(firstPlayer.Color, GameConstants.INITIAL_MAJOR_ROW_OF_FIRST_PLAYER);
            AddPawnsOnBoard(firstPlayer.Color, GameConstants.INITIAL_PAWNS_ROW_OF_FIRST_PLAYER);

            AddMajorPiecesOnBoard(secondPlayer.Color, GameConstants.INITIAL_MAJOR_ROW_OF_SECOND_PLAYER);
            AddPawnsOnBoard(secondPlayer.Color, GameConstants.INITIAL_PAWNS_ROW_OF_SECOND_PLAYER);

        }

        private void AddPawnsOnBoard(ChessColor color, int row) {
            for (int i = 0; i < BoardConstants.STANDARD_BOARD_ROWS; i++) {
                Pawn pawn = (Pawn)ChessFactory.CreatePiece(PieceType.PAWN, color, ChessFactory.CreatePosition(row, i));
                PlaceNewPiece(pawn, ChessFactory.CreateChessPosition((char)(i + 'A'), row));
            }
        }

        private void AddMajorPiecesOnBoard(ChessColor color, int row) {
            for (int i = 0; i < BoardConstants.STANDARD_BOARD_ROWS; i++) {
                PieceType type = GameConstants.MAJOR_PIECES_SEQUENCE.ElementAt(i);
                BasePiece piece = ChessFactory.CreatePiece(type, color, ChessFactory.CreatePosition(row, i));
                PlaceNewPiece(piece, ChessFactory.CreateChessPosition((char)(i + 'A'), row));
            }
        }


        // Helper methods
        private IPlayer FindPlayer(ChessColor color) {
            return Players[color];
        }

        private IPlayer FindOpponentPlayer() {
            return CurrentPlayer.Color == ChessColor.WHITE ? BlackPlayer : WhitePlayer;
        }

        private IPlayer FindOpponentPlayer(IPlayer player) {
            return player.Color == ChessColor.WHITE ? BlackPlayer : WhitePlayer;
        }

        private IList<BasePiece> FetchPieces(IPlayer player) {
            return Pieces[player];
        }

        private IList<BasePiece> FetchOpponentPieces() {
            return Pieces[FindOpponentPlayer()];
        }

        private IList<BasePiece> FetchOpponentPieces(IPlayer player) {
            return Pieces[FindOpponentPlayer(player)];
        }

        private ICell FindKing(ChessColor color) {
            foreach (ICell cell in Board.Grid) {
                BasePiece piece = cell.Piece;
                if (!Common.IsObjectNull(piece) && piece.Type == PieceType.KING && piece.Color == color) {
                    return cell;
                }
            }

            throw new ChessException("There is no king on the board");
        }

        private void AddPieceOnPlayer(BasePiece piece) {
            IPlayer player = FindPlayer(piece.Color);
            Pieces[player].Add(piece);
        }

        private void PlaceNewPiece(BasePiece piece, ChessPosition chessPosition) {
            Board.PlacePiece(piece, Board.GetCell(chessPosition.ToPosition()));
            AddPieceOnPlayer(piece);
        }

        private bool CheckEnPassant(IMovement move) {
            return move.MovedPiece.Type == PieceType.PAWN;
        }

        private bool IsEnPassantVulnerable(IMovement move) {
            return move.MovedPiece.Type == PieceType.PAWN && (move.Target.Position.Row == move.Source.Position.Row - 2 || move.Target.Position.Row == move.Source.Position.Row + 2);
        }

        private bool IsCastlingMove(IMovement move) {
            return IsRightCastlingMove(move) || IsLeftCastlingMove(move);

        }

        private bool IsRightCastlingMove(IMovement move) {
            return move.MovedPiece.Type == PieceType.KING && move.Target.Position.Column == move.Source.Position.Column + 2;

        }

        private bool IsLeftCastlingMove(IMovement move) {
            return move.MovedPiece.Type == PieceType.KING && move.Target.Position.Column == move.Source.Position.Column - 2;

        }

        private BasePiece ReplacePromotedPiece(PieceType type) {
            if (Promoted == null) {
                throw new ChessException("There is no piece to be promoted");

            }
            if (type != PieceType.BISHOP && type != PieceType.KNIGHT && type != PieceType.ROOK && type != PieceType.QUEEN) {
                throw new ChessException("Invalid type for promotion");

            }

            ICell cell = Board.GetCell(Promoted.Position);
            BasePiece piece = Board.RemovePiece(cell.Position);
            Pieces[CurrentPlayer].Remove(piece);

            BasePiece replacedPiece = ChessFactory.CreatePiece(type, piece.Color, cell.Position);
            Board.PlacePiece(replacedPiece, cell);
            Pieces[CurrentPlayer].Add(replacedPiece);

            return replacedPiece;

        }
 
    }
}
