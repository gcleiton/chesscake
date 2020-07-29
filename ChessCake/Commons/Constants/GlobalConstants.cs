using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Commons.Constants {
    class GlobalConstants {

        public const string PLAYER_NAME_FORMATTER_INPUT = "What is the name of the {0}º player? ";
        public const string FIRST_MESSAGE_ON_CHARACTER = "\n\n           /  {0}!\n";
        public const string SECOND_MESSAGE_ON_CHARACTER = "         / {0}\n";
        public const string DEFAULT_ASTERISKS_DIVIDER = "**********************";

        public const int MIN_ROW_VALUE_ON_GRID = 0;
        public const int MAX_ROW_VALUE_ON_GRID = 7;
        public const int MIN_COLUMN_VALUE_ON_GRID = 0;
        public const int MAX_COLUMN_VALUE_ON_GRID = 7;

        public const int MIN_ROW_VALUE_ON_BOARD = 1;
        public const int MAX_ROW_VALUE_ON_BOARD = 8;
        public const char MIN_COLUMN_VALUE_ON_BOARD = 'A';
        public const char MAX_COLUMN_VALUE_ON_BOARD = 'H';

        public const int STANDARD_BOARD_COLUMNS = 8;
        public const int STANDARD_BOARD_ROWS = 8;

        public const int STANDARD_NUMBER_OF_PLAYERS_GAME = 2;
    }
}
