using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChess.Pieces
{
    class Knight : ChessPiece
    {
        public Knight(int _row, int _col, bool isWhite)
        {
            this.row = _row;
            this.col = _col;
            this.Image = isWhite ? new Bitmap(Properties.Resources.whiteKnight) : new Bitmap(Properties.Resources.blackKnight);
            _ = isWhite ? this.IsWhite = true : this.IsBlack = true;
        }

        public override bool IsCanMove(ChessBoard Board, int toR, int toC, bool onlyCheck)
        {
            int dx = toC - col;
            int dy = toR - row;
            if ((Math.Abs(dx) == 2 && Math.Abs(dy) == 1) || (Math.Abs(dx) == 1 && Math.Abs(dy) == 2))
            {
                if (Board[toR, toC] == null) return true;       // If empty
                if ((this.IsWhite ? Board[toR, toC].IsBlack : Board[toR, toC].IsWhite)) return true;       // If the figure is the opposite color
            }
            return false;
        }

    }
}
