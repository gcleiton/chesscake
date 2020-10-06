using ChessCake.Commons;
using ChessCake.Commons.Constants;
using ChessCake.Models.ChessPositions.Contracts;
using System;
using System.Text;

namespace ChessCake.Models.Positions.Chess {
    public class ChessPosition : IChessPosition {
        public char Column { get; protected set; }
        public int Row { get; protected set; }

        public ChessPosition(char column, int row) {
            this.Column = column;
            this.Row = row;
        }

        public Position ToPosition() {
            return ChessFactory.CreatePosition(8 - Row, Char.ToUpper(Column) - 'A');

        }

        public static bool isValidChessPosition(ChessPosition chessPosition) {
            return (chessPosition.Column >= BoardConstants.MIN_COLUMN_VALUE_ON_BOARD && chessPosition.Column <= BoardConstants.MAX_COLUMN_VALUE_ON_BOARD
                 && chessPosition.Row >= BoardConstants.MIN_ROW_VALUE_ON_BOARD && chessPosition.Row <= BoardConstants.MAX_ROW_VALUE_ON_BOARD);
        }

        public static bool isValidCoordinates(char column, int row) {
            return (column >= BoardConstants.MIN_COLUMN_VALUE_ON_BOARD && column <= BoardConstants.MAX_COLUMN_VALUE_ON_BOARD
                && row >= BoardConstants.MIN_ROW_VALUE_ON_BOARD && row <= BoardConstants.MAX_ROW_VALUE_ON_BOARD);
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
