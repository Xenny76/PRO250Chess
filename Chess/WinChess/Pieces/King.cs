using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChess.Pieces
{
    class King : ChessPiece
    {
        public King(int _row, int _col, bool isWhite)
        {
            this.row = _row;
            this.col = _col;
            this.Image = isWhite ? new Bitmap(Properties.Resources.whiteKing) : new Bitmap(Properties.Resources.blackKing);
            _ = isWhite ? this.IsWhite = true : this.IsBlack = true;
        }

        public override bool IsCanMove(ChessBoard Board, int toR, int toC, bool onlyCheck)
        {
            int dx = toC - col;
            int dy = toR - row;

            // If the figure is the same color at the end, then you can't move.
            if (Board[toR, toC] != null && (this.IsWhite ? Board[toR, toC].IsWhite : Board[toR, toC].IsBlack))
                return false;

            if (dy == 0 && dx == 2) // Check if castling is to the right. There can only be a rook with the hasMoved flag turned off.
            {
                if (!hasMoved && Board[7, 7] != null && !Board[7, 7].hasMoved)
                {
                    for (int c = col + 1; c <= toC; c++)
                    {
                        if (Board[row, c] != null) return false;
                    }

                    // Will there be a Check?
                    col++;
                    if (IsInCheck(Board))
                    {
                        col--;
                        return false;
                    }
                    col++;
                    if (IsInCheck(Board))
                    {
                        col -= 2;
                        return false;
                    }
                    col -= 2;

                    // We are castling
                    if (!onlyCheck)
                    {
                        ChessPiece rook = Board[7, 7];
                        Board[7, 5] = rook;
                        Board[7, 7] = null;
                        rook.col = 5;
                        rook.hasMoved = true;
                    }
                    return true;
                }
                else return false;
            }
            else if (dy == 0 && dx == -2) // Check if there is a left castling. There can only be a rook with the hasMoved flag turned off.
            {
                if (!hasMoved && Board[7, 0] != null && !Board[7, 0].hasMoved)
                {
                    for (int c = toC; c < col; c++)
                    {
                        if (Board[row, c] != null) return false;
                    }

                    // Will there be a Check?
                    col--;
                    if (IsInCheck(Board))
                    {
                        col++;
                        return false;
                    }
                    col--;
                    if (IsInCheck(Board))
                    {
                        col += 2;
                        return false;
                    }
                    col += 2;

                    // We are castling
                    if (!onlyCheck)
                    {
                        ChessPiece rook = Board[7, 0];
                        Board[7, 3] = rook;
                        Board[7, 0] = null;
                        rook.col = 3;
                        rook.hasMoved = true;
                    }
                    return true;
                }
                else return false;
            }
            else if ((Math.Abs(dx) > 1) || (Math.Abs(dy) > 1)) // If the diagonal move is more than one cell then "it is not allowed"
                return false;

            return true;
        }

        public override bool IsInCheck(ChessBoard Board)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    if (Board[r, c] != null && (this.IsWhite ? Board[r, c].IsBlack : Board[r, c].IsWhite))
                    {
                        if (Board[r, c].IsCanMove(Board, row, col, true)) return true;  // We check if an opposite color piece can move to our cell, then there will be a check.
                    }
                }
            }
            return false;
        }

    }
}
