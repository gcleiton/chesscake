﻿using ChessCake.Commons;
using ChessCake.Models.Boards.Cells.Contracts;
using ChessCake.Models.Pieces.Contracts;
using ChessCake.Models.Positions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessCake.Models.Boards.Cells {
    public class Cell : ICell {
        public Position Position { get; protected set; }
        public BasePiece Piece { get; protected set; }

        //public bool isOccupied { get; set; }

        public Cell(Position position, BasePiece piece) {
            this.Position = position;
            this.Piece = piece;
        
        }

        public bool isOccupied() {
            return Common.isObjectNull(Piece);
        }

        public void placePiece(BasePiece piece) {
            //if (Common.isObjectNull(piece)) this.Piece.isAvailable = false;

            this.Piece = piece;
        }

        public BasePiece releasePiece() {
            BasePiece releasedPiece = Piece;
            Piece = null;

            return releasedPiece;
        }

        public override string ToString() {
            return Position.Column + " | " + Position.Row;
        }

    }
}
