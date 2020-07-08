using ChessCake.Commons;
using ChessCake.Models.ChessPositions.Contracts;
using System;
using System.Text;

namespace ChessCake.Models.Positions.Chess {
    public class ChessPosition : IChessPosition {
        public char Column { get; protected set; }
        public int Row { get; protected set; }

        public ChessPosition(char column, int row) {

        }

        public Position ToPosition() {
            return new Position(8 - Row, Char.ToUpper(Column) - 'A');
        }

        public static bool isValidChessPosition(ChessPosition chessPosition) {
            return (chessPosition.Column >= GlobalConstants.MIN_COLUMN_VALUE_ON_BOARD && chessPosition.Column <= GlobalConstants.MAX_COLUMN_VALUE_ON_BOARD
                 && chessPosition.Row >= GlobalConstants.MIN_ROW_VALUE_ON_BOARD && chessPosition.Row <= GlobalConstants.MAX_ROW_VALUE_ON_BOARD);
        }

        public static bool isValidCoordinates(char column, int row) {
            return (column >= GlobalConstants.MIN_COLUMN_VALUE_ON_BOARD && column <= GlobalConstants.MAX_COLUMN_VALUE_ON_BOARD
                && row >= GlobalConstants.MIN_ROW_VALUE_ON_BOARD && row <= GlobalConstants.MAX_ROW_VALUE_ON_BOARD);
        }

        public override string ToString() {
            StringBuilder msg = new StringBuilder();
            msg.Append("Chess Position: ");
            msg.Append(Char.ToUpper(Column));
            msg.Append(Row);

            return msg.ToString();
        }
    }
}
