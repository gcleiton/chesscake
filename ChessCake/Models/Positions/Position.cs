
using ChessCake.Commons;
using ChessCake.Commons.Constants;
using ChessCake.Exceptions;
using ChessCake.Models.Positions.Contracts;
using Colorful;
using System.Text;

namespace ChessCake.Models.Positions {
    public class Position : IPosition {
        public int Row { get; set; }
        public int Column { get; set; }

        public Position(int row, int column) {
            //if (!isValidCoordinates(row, column)) {
            //    throw new ChessException("Coordenadas da posição inválida!");
            //}

            this.Row = row;
            this.Column = column;
        }

        public void SetCoordinates(int row, int column) {
            Row = row;
            Column = column;
        }

        public static bool IsValidPosition(IPosition position) {

            return IsValidCoordinates(position.Row, position.Column);
        }

        public static bool IsValidCoordinates(int row, int column) {
            return row >= GlobalConstants.MIN_ROW_VALUE_ON_GRID && row <= GlobalConstants.MAX_ROW_VALUE_ON_GRID
                && column >= GlobalConstants.MIN_COLUMN_VALUE_ON_GRID && column <= GlobalConstants.MAX_COLUMN_VALUE_ON_GRID;
        }

        public override string ToString() {
            StringBuilder msg = new StringBuilder();
            msg.Append("Position: (");
            msg.Append(this.Row).Append(",");
            msg.Append(this.Column).Append(")");

            return msg.ToString();
        }
    }
}
