using ChessCake.Commons;
using ChessCake.Commons.Enumerations;
using ChessCake.Engines.Contracts;
using ChessCake.Models.Boards.Cells;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Boards.Contracts;
using ChessCake.Models.Pieces;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Positions;
using ChessCake.Providers.Movements.Pieces.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace ChessCake.Providers.Movements {
    class MovementProvider {

        public static IList<ICell> GenerateLegalMoves(IEngine engine, ICell source) {
            BasePiece piece = source.Piece;

            if (Common.IsObjectNull(piece)) return null;

            IList<ICell> legalMoves = new List<ICell>();

            switch (piece.Type) {
                case PieceType.PAWN:
                    break;

                case PieceType.KNIGHT:
                    break;

                case PieceType.BISHOP:
                    legalMoves = GenerateBishopMoves(engine, source);
                    break;

                case PieceType.ROOK:
                    break;

                case PieceType.QUEEN:
                    break;

                case PieceType.KING:
                    break;

                default:
                    break;
            }

            return legalMoves;

        }

        public static IList<ICell> GenerateBishopMoves(IEngine engine, ICell source) {
            IPieceMovement bishopMovement = ChessFactory.CreateBishopMovement(engine, source);

            return bishopMovement.GenerateLegalMoves();
        
        }



    }

}
