
using ChessCake.Commons;
using ChessCake.Exceptions;
using ChessCake.Models.Positions.Contracts;
using System.Text;

namespace ChessCake.Models.Positions {
    public class Position : IPosition {
        public int Row { get; set; }
        public int Column { get; set; }

        public Position(int row, int column) {
            if (!isValidCoordinates(row, column)) {
                throw new ChessException("Coordenadas da posição inválida!");
            }

            this.Row = row;
            this.Column = column;
        }

        public static bool isValidPosition(Position position) {
            return (position.Row >= GlobalConstants.MIN_ROW_VALUE_ON_GRID && position.Row <= GlobalConstants.MAX_ROW_VALUE_ON_GRID
                && position.Column >= GlobalConstants.MIN_ROW_VALUE_ON_GRID && position.Column <= GlobalConstants.MAX_ROW_VALUE_ON_GRID);
        }

        public static bool isValidCoordinates(int row, int column) {
            return (row >= GlobalConstants.MIN_ROW_VALUE_ON_GRID && row <= GlobalConstants.MAX_ROW_VALUE_ON_GRID
                && column >= GlobalConstants.MIN_COLUMN_VALUE_ON_GRID && column <= GlobalConstants.MAX_COLUMN_VALUE_ON_GRID);
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
