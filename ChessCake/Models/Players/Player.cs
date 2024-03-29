﻿using ChessCake.Commons;
using ChessCake.Commons.Enumerations;
using ChessCake.Exceptions;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Players.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Players {
    public class Player : IPlayer {
        public string Name { get; set; }
        public ChessColor Color { get; set; }
        public IList<BasePiece> Pieces { get; set; }

        public Player(string name, ChessColor color) {
            this.Name = name;
            this.Color = color;
            Pieces = new List<BasePiece>();
        }

        public void AddPiece(BasePiece piece) {
            Console.WriteLine(piece);
            if (Common.IsObjectNull(piece)) {
                throw new ChessException("Invalid Piece!");
            }
            if (isPieceExists(piece)) {
                throw new ChessException("The player already has this piece.");
            }

            Pieces.Add(piece);
        }

        public void removePiece(BasePiece piece) {
            if (Common.IsObjectNull(piece)) {
                throw new ChessException("Invalid Piece!");
            }

            if (!isPieceExists(piece)) {
                throw new ChessException("The player already has this piece.");
            }

            Pieces.Remove(piece);
        }

        public bool isPieceExists(BasePiece piece) {
            return Pieces.Contains(piece);
        }

    }
}
