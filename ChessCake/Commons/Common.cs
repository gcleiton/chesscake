using ChessCake.Commons.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessCake.Commons {
    class Common {
        public static Boolean isObjectNull(object obj) {
            if (obj == null) return true;
            return false;
        }

        public static IList<ChessColor> GenerateOrderPlayers() {
            Random random = new Random();
            IList<ChessColor> order = Enum.GetValues(typeof(ChessColor)).Cast<ChessColor>().ToList();
            order.Remove(ChessColor.UNDEFINED);

            IList<ChessColor> shuffledList = order.OrderBy(x => random.Next(order.Count())).ToList();

            return shuffledList;

        }

        public static void QuitGame() {
            Environment.Exit(1);
        }

    }
}
