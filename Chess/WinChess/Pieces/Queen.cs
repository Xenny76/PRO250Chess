using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChess.Pieces
{
    class Queen : ChessPiece
    {
        public Queen(int _row, int _col, bool isWhite)
        {
            this.row = _row;
            this.col = _col;
            this.Image = isWhite ? new Bitmap(Properties.Resources.whiteQueen) : new Bitmap(Properties.Resources.blackQueen);
            _ = isWhite ? this.IsWhite = true : this.IsBlack = true;
        }

        public override bool IsCanMove(ChessBoard Board, int toR, int toC, bool onlyCheck)
        {
            int dx = toC - col;
            int dy = toR - row;

            // Checking if the queen can move towards the cell
            if (!(
              (dx == 0 && dy != 0) ||
              (dx != 0 && dy == 0) ||
              (Math.Abs(dx) == Math.Abs(dy))))
            {
                return false;
            }

            // If the figure is the same color at the end, then you can't move.
            if (Board[toR, toC] != null && (this.IsWhite ? Board[toR, toC].IsWhite : Board[toR, toC].IsBlack))
                return false;

            // Checking cells between path points
            // vertically
            if (dx == 0 && dy != 0)
            {
                int ddy = dy / Math.Abs(dy);
                for (int r = row + ddy; r != toR; r += ddy)
                {
                    if (Board[r, col] != null) return false;
                }
            }

            // horizontally
            if (dx != 0 && dy == 0)
            {
                int ddx = dx / Math.Abs(dx);
                for (int c = col + ddx; c != toC; c += ddx)
                {
                    if (Board[row, c] != null) return false;
                }
            }
            if (dx != 0 && dy != 0)
            {
                int ddx = dx / Math.Abs(dx);
                int ddy = dy / Math.Abs(dy);
                for (int i = 1; i < Math.Abs(dx); i++)
                {
                    if (Board[row + (i * ddy), col + (i * ddx)] != null) return false;
                }
            }

            return true;
        }

    }
}
