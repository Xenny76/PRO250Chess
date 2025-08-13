using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChess.Pieces
{
    class Bishop : ChessPiece
    {
        public Bishop(int _row, int _col, bool isWhite)
        {
            this.row = _row;
            this.col = _col;
            this.Image = isWhite ? new Bitmap(Properties.Resources.whiteBishop) : new Bitmap(Properties.Resources.blackBishop);
            _ = isWhite ? this.IsWhite = true : this.IsBlack = true;
        }

        public override bool IsCanMove(ChessBoard Board, int toR, int toC, bool onlyCheck)
        {
            int dx = toC - col;
            int dy = toR - row;
            if (Math.Abs(dx) != Math.Abs(dy)) return false; // The bishop moves only diagonally.

            if (Board[toR, toC] != null && (this.IsWhite ? Board[toR, toC].IsWhite : Board[toR, toC].IsBlack))  // If the figure is the same color at the end, then you can't move.
                return false;
            
            int ddx = dx / Math.Abs(dx);
            int ddy = dy / Math.Abs(dy);
            for (int i = 1; i < Math.Abs(dx); i++)          // We go diagonally from the starting position to the end
            {
                if (Board[row + (i * ddy), col + (i * ddx)] != null) return false; // If there is a figure on the way, then we stop
            }

            return true;
        }
    }
}
