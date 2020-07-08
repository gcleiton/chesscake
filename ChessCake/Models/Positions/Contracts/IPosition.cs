using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Positions.Contracts {
    interface IPosition {
        public int Row { get; }
        public int Column { get; }
    }
}
