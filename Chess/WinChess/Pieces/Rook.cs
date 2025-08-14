using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChess.Pieces
{
    class Rook : ChessPiece
    {
        public Rook(int _row, int _col, bool isWhite)
        {
            this.row = _row;
            this.col = _col;
            this.Image = isWhite ? new Bitmap(Properties.Resources.whiteRook) : new Bitmap(Properties.Resources.blackRook);
            _ = isWhite ? this.IsWhite = true : this.IsBlack = true;
        }

        public override bool IsCanMove(ChessBoard Board, int toR, int toC, bool onlyCheck)
        {
            if (row != toR && col != toC) { return false; }

            // If the figure is the same color at the end, then you can't move.
            if (Board[toR, toC] != null && (this.IsWhite ? Board[toR, toC].IsWhite : Board[toR, toC].IsBlack))
                return false;

            if (row == toR)       // We check that there are no figures vertically
            {
                int dx = toC - col;
                int ddx = dx / Math.Abs(dx);
                for (int c = col + ddx; c != toC; c += ddx)
                {
                    if (Board[row, c] != null) return false;
                }
            }
            else if (col == toC)  // We check that there are no figures horizontally
            {
                int dy = toR - row;
                int ddy = dy / Math.Abs(dy);
                for (int r = row + ddy; r != toR; r += ddy)
                {
                    if (Board[r, col] != null) return false;
                }
            }
            return true;
        }

    }
}
