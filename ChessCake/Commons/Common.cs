﻿using ChessCake.Commons.Enumerations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ChessCake.Commons {
    class Common {

        //public static TextInfo StringFormatter = new CultureInfo("en-US", false).TextInfo;

        public static bool IsObjectNull(object obj) {
            if (obj == null) return true;
            return false;
        }

        public static IList<ChessColor> GenerateOrderPlayers() { // sorts players randomly
            Random random = new Random();
            IList<ChessColor> order = Enum.GetValues(typeof(ChessColor)).Cast<ChessColor>().ToList();
            order.Remove(ChessColor.UNDEFINED);

            IList<ChessColor> shuffledList = order.OrderBy(x => random.Next(order.Count())).ToList();

            return shuffledList;

        }

        public static void ClearConsole() {
            Console.Clear();
        }

        public static void QuitGame() { // stop execution program
            ClearConsole();
            Environment.Exit(1);
        }

    }
}
