using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Positions.Contracts {
    public interface IPosition {
        public int Row { get; }
        public int Column { get; }
    }
}
