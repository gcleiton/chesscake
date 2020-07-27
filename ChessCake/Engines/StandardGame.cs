using ChessCake.Commons;
using ChessCake.Commons.Enumerations;
using ChessCake.Engines.Contracts;
using ChessCake.Engines.Screens;
using ChessCake.Exceptions;
using ChessCake.Models.Boards.Contracts;
using ChessCake.Models.Movements.Contracts;
using ChessCake.Models.Pieces;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Players;
using ChessCake.Models.Players.Contracts;
using ChessCake.Models.Positions.Chess;
using ChessCake.Models.Positions.Contracts;
using ChessCake.Providers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace ChessCake.Engines {
    class StandardGame : IEngine {
        public IBoard Board { get; private set; }

        private readonly IDictionary<ChessColor, IPlayer> Players; // use OrderedDictionary

        public IPlayer CurrentPlayer { get; set; }

        private int Turn;

        private IList<BasePiece> CapturedPieces;

        public StandardGame(IDictionary<ChessColor, IPlayer> players) {
            Board = ChessFactory.CreateBoard();

            this.Players = players;
            this.CapturedPieces = new List<BasePiece>();
            this.Turn = 1;
            this.CurrentPlayer = FindPlayer(ChessColor.WHITE);

            this.ValidateStandardGame();

            InitBoard();
        }

        public void Initialize() {
            while (true) {
                try {
                    Console.Clear();

                    Screen.PrintBoard(Board);

                    IMovement nextMove = InputProvider.ReadMove(Board);

                    Console.WriteLine(nextMove.Source.Position);
                    Console.WriteLine(nextMove.Target.Position);

                    break;

                    //IPosition source = Screen.ReadChessPosition().ToPosition();

                    //List<Cell> possibleMoves = match.legalMoves(source);

                    //Console.Clear();

                    //Screen.printBoard(match.board, possibleMoves);

                    //Console.WriteLine();

                    //Position target = Screen.readChessPosition(false).ToPosition();

                    //match.performChessMove(match.board.getCell(source), match.board.getCell(target));
                } catch (ChessException e) {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
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
            AddPieceOnPlayer(piece.Color, piece);
            Console.WriteLine("test");

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


        //    public ChessEngine(Dictionary<Color, Player> players) {
        //        this.players = players;
        //        board = new Board();
        //        capturedPieces = new List<Piece>();
        //        turn = 1;
        //        currentPlayer = findPlayer(Color.WHITE);

        //        whitePlayer = this.players[Color.WHITE];
        //        blackPlayer = this.players[Color.BLACK];

        //        initBoard();
        //    }

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

        //    private void removePieceOnPlayer(Piece piece) {
        //        if (piece.color == Color.WHITE) {
        //            whitePlayer.removePiece(piece);
        //        } else {
        //            blackPlayer.removePiece(piece);
        //        }

        //        updatePlayers(whitePlayer, blackPlayer);
        //    }

        //    private void validateSourceCell(Cell source) {
        //        if (!source.isOccupied()) {
        //            throw new ChessException("Não existe nenhuma peça na posição de origem informado.");
        //        }
        //        if (currentPlayer != findPlayer(source.piece.color)) {
        //            throw new ChessException("A peça selecionada não pertence a você.");
        //        }
        //        if (!source.piece.isThereAnyLegalMove()) {
        //            throw new ChessException("Não existe movimentos possíveis para a peça selecionada.");
        //        }
        //    }

        //    private void validateTargetCell(Cell source, Cell target) {
        //        if (!source.piece.isLegalMove(target)) {
        //            throw new ChessException("A peça selecionada não pode mover para essa posição.");
        //        }
        //    }

        //    public List<Cell> legalMoves(Position sourcePosition) {
        //        Cell sourceCell = board.getCell(sourcePosition);
        //        validateSourceCell(sourceCell);
        //        return sourceCell.piece.generateLegalMoves();
        //    }

        //    public Move performChessMove(Cell source, Cell target) {
        //        validateSourceCell(source);
        //        validateTargetCell(source, target);

        //        Move move = makeMove(source, target);

        //        nextTurn();

        //        return move;

        //    }

        //    public Move makeMove(Cell source, Cell target) {
        //        Piece movedPiece = board.removePiece(source.position);
        //        Piece capturedPiece = board.removePiece(target.position);
        //        board.placePiece(movedPiece, target);

        //        if (capturedPiece != null) {
        //            Console.WriteLine(capturedPiece);
        //            capturedPieces.Add((Piece)capturedPiece);

        //            removePieceOnPlayer(capturedPiece);
        //        }

        //        return new Move(currentPlayer, source, target);
        //    }

        //    private void placeNewPiece(Piece piece, char column, int row) {
        //        board.placePiece(piece, board.getCell(new ChessPosition(column, row).ToPosition()));
        //        if (piece.color == Color.WHITE) {
        //            players[Color.WHITE].addPiece(piece);
        //        } else {
        //            players[Color.BLACK].addPiece(piece);
        //        }
        //    }

        //    private void initBoard() {
        //        placeNewPiece(new Rook(Color.WHITE, true, this), 'A', 1);
        //        placeNewPiece(new Knight(Color.WHITE, true, this), 'B', 1);
        //        placeNewPiece(new Bishop(Color.WHITE, true, this), 'C', 1);
        //        placeNewPiece(new King(Color.WHITE, true, this), 'D', 1);
        //        placeNewPiece(new Queen(Color.WHITE, true, this), 'E', 1);
        //        placeNewPiece(new Bishop(Color.WHITE, true, this), 'F', 1);
        //        placeNewPiece(new Knight(Color.WHITE, true, this), 'G', 1);
        //        placeNewPiece(new Rook(Color.WHITE, true, this), 'H', 1);
        //        /*placeNewPiece(new Pawn(Color.WHITE, true, this), 'A', 2);
        //        placeNewPiece(new Pawn(Color.WHITE, true, this), 'B', 2); 
        //        placeNewPiece(new Pawn(Color.WHITE, true, this), 'C', 2);
        //        placeNewPiece(new Pawn(Color.WHITE, true, this), 'D', 2);
        //        placeNewPiece(new Pawn(Color.WHITE, true, this), 'E', 2);
        //        placeNewPiece(new Pawn(Color.WHITE, true, this), 'F', 2);
        //        placeNewPiece(new Pawn(Color.WHITE, true, this), 'G', 2);
        //        placeNewPiece(new Pawn(Color.WHITE, true, this), 'H', 2);*/

        //        placeNewPiece(new Rook(Color.BLACK, true, this), 'A', 8);
        //        placeNewPiece(new Knight(Color.BLACK, true, this), 'B', 8);
        //        placeNewPiece(new Bishop(Color.BLACK, true, this), 'C', 8);
        //        placeNewPiece(new King(Color.BLACK, true, this), 'D', 8);
        //        placeNewPiece(new Queen(Color.BLACK, true, this), 'E', 8);
        //        placeNewPiece(new Bishop(Color.BLACK, true, this), 'F', 8);
        //        placeNewPiece(new Knight(Color.BLACK, true, this), 'G', 8);
        //        placeNewPiece(new Rook(Color.BLACK, true, this), 'H', 8);
        //        /*placeNewPiece(new Pawn(Color.BLACK, true, this), 'A', 7);
        //        placeNewPiece(new Pawn(Color.BLACK, true, this), 'B', 7);
        //        placeNewPiece(new Pawn(Color.BLACK, true, this), 'C', 7);
        //        placeNewPiece(new Pawn(Color.BLACK, true, this), 'D', 7);
        //        placeNewPiece(new Pawn(Color.BLACK, true, this), 'E', 7);
        //        placeNewPiece(new Pawn(Color.BLACK, true, this), 'F', 7);
        //        placeNewPiece(new Pawn(Color.BLACK, true, this), 'G', 7);
        //        placeNewPiece(new Pawn(Color.BLACK, true, this), 'H', 7);*/

        //    }

        //}

    }
}
