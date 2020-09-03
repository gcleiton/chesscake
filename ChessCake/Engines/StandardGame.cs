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
using System.Runtime.Serialization;

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

        public bool InCheckmate { get; private set; }

        public BasePiece EnPassant { get; private set; }

        public BasePiece Promoted { get; private set; }

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
            while (!InCheckmate) {
                //try {

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

                    movementProvider.Update(this);
                
                //} catch (ChessException e) {
                //    Console.WriteLine(e.Message);
                //    Console.ReadLine();
                //}
            }

            Common.ClearConsole();
            Screen.PrintMatch(this);

        }

        private void HandlePromotedPiece() { // Verificar o motivo dele não perguntar antes para qual posição a peça deseja mover
            if(!Common.IsObjectNull(Promoted)) {
                PieceType type = InputProvider.ReadPromotedPiece();
                ReplacePromotedPiece(type);
            }
        }

        private void PerformMove(IMovement move) {
            ICell source = move.Source;
            ICell target = move.Target;

            ValidateSource(source);
            ValidateTarget(source, target);

            move = MakeMove(move);

            movementProvider.Update(this);

            Promoted = null;
            if (move.MovedPiece.Type == PieceType.PAWN) {
                if(move.MovedPiece.Color == ChessColor.WHITE && move.Target.Position.Row == 0 ||
                    move.MovedPiece.Color == ChessColor.BLACK && move.Target.Position.Row == 7) {

                    Promoted = Board.FindPiece(move.Target);
                    Promoted = ReplacePromotedPiece(PieceType.QUEEN);
                }
            }

            ValidateCheck(move);

            if(ValidateCheckMate(FindOpponentPlayer())) InCheckmate = true;

            else NextTurn();

            // Special move - En Passant

            if (IsEnPassantVulnerable(move)) EnPassant = move.MovedPiece;

            else EnPassant = null;

            //movementProvider.Update(this);

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

        

        private bool IsEnPassantVulnerable(IMovement move) {
            return move.MovedPiece.Type == PieceType.PAWN && (move.Target.Position.Row == move.Source.Position.Row - 2 || move.Target.Position.Row == move.Source.Position.Row + 2);
        }

        private bool CheckEnPassant(IMovement move) {
            return move.MovedPiece.Type == PieceType.PAWN;
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

        private IMovement HandleEnPassantMove(IMovement move, bool isUndo = false) {

            if (!isUndo) return MakeEnPassantMove(move);

            return UndoEnPassantMove(move);
        }

        private bool IsCastlingMove(IMovement move) {
            return IsRightCastlingMove(move) || IsLeftCastlingMove(move);

        }

        private bool IsRightCastlingMove(IMovement move) {
            return move.MovedPiece.Type == PieceType.KING && move.Target.Position.Column == move.Source.Position.Column + 2;

        }

        private bool IsLeftCastlingMove(IMovement move) {
            return move.MovedPiece.Type == PieceType.KING && move.Target.Position.Column == move.Source.Position.Column -2;

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

        private IMovement HandleCastlingMove(IMovement kingMove, bool IsUndo = false) {
            if(IsRightCastlingMove(kingMove)) {
                if (IsUndo) return UndoCastlingMove(CastlingDirection.RIGHT, kingMove);
                return MakeCastlingMove(CastlingDirection.RIGHT, kingMove);
            }

            if (IsUndo) return UndoCastlingMove(CastlingDirection.LEFT, kingMove);

            return MakeCastlingMove(CastlingDirection.LEFT, kingMove);

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
                CapturedPieces[FindOpponentPlayer(move.Player)].Remove(capturedPiece);
            }

            //if (IsCastlingMove(move)) {
            //    HandleCastlingMove(move, true);
            //}

            if(CheckEnPassant(move)) {
                HandleEnPassantMove(move, true);
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

            foreach(BasePiece piece in playerPieces.ToList()) {
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

            if (validateSource) ValidateSource(sourceCell);

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

            BasePiece piece = ChessFactory.CreatePiece(PieceType.PAWN, firstPlayer.Color, ChessFactory.CreateChessPosition('b', 2).ToPosition());
            PlaceNewPiece(piece, ChessFactory.CreateChessPosition('b', 2));

            piece = ChessFactory.CreatePiece(PieceType.PAWN, secondPlayer.Color, ChessFactory.CreateChessPosition('f', 7).ToPosition());
            PlaceNewPiece(piece, ChessFactory.CreateChessPosition('f', 7));

            //BasePiece piece = ChessFactory.CreatePiece(PieceType.ROOK, secondPlayer.Color, ChessFactory.CreateChessPosition('a', 1).ToPosition());
            //PlaceNewPiece(piece, ChessFactory.CreateChessPosition('a', 1));

            //piece = ChessFactory.CreatePiece(PieceType.ROOK, secondPlayer.Color, ChessFactory.CreateChessPosition('h', 1).ToPosition());
            //PlaceNewPiece(piece, ChessFactory.CreateChessPosition('h', 1));

            piece = ChessFactory.CreatePiece(PieceType.KING, secondPlayer.Color, ChessFactory.CreateChessPosition('e', 1).ToPosition());
            PlaceNewPiece(piece, ChessFactory.CreateChessPosition('e', 1));

            //piece = ChessFactory.CreatePiece(PieceType.ROOK, firstPlayer.Color, ChessFactory.CreateChessPosition('a', 8).ToPosition());
            //PlaceNewPiece(piece, ChessFactory.CreateChessPosition('a', 8));

            //piece = ChessFactory.CreatePiece(PieceType.ROOK, firstPlayer.Color, ChessFactory.CreateChessPosition('h', 8).ToPosition());
            //PlaceNewPiece(piece, ChessFactory.CreateChessPosition('h', 8));

            piece = ChessFactory.CreatePiece(PieceType.KING, firstPlayer.Color, ChessFactory.CreateChessPosition('e', 8).ToPosition());
            PlaceNewPiece(piece, ChessFactory.CreateChessPosition('e', 8));

            //AddMajorPiecesOnBoard(firstPlayer.Color, GameConstants.INITIAL_MAJOR_ROW_OF_FIRST_PLAYER);
            //AddPawnsOnBoard(firstPlayer.Color, GameConstants.INITIAL_PAWNS_ROW_OF_FIRST_PLAYER);

            //AddMajorPiecesOnBoard(secondPlayer.Color, GameConstants.INITIAL_MAJOR_ROW_OF_SECOND_PLAYER);
            //AddPawnsOnBoard(secondPlayer.Color, GameConstants.INITIAL_PAWNS_ROW_OF_SECOND_PLAYER);

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

    [Serializable]
    internal class IllegalStateException : Exception {
        public IllegalStateException() {
        }

        public IllegalStateException(string message) : base(message) {
        }

        public IllegalStateException(string message, Exception innerException) : base(message, innerException) {
        }

        protected IllegalStateException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
