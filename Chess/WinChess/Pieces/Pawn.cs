using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChess.Pieces
{
    class Pawn : ChessPiece
    {
        public bool isJump = false;       // The flag indicates that the pawn jumped over a square on its first move.

        public Pawn(int _row, int _col, bool isWhite)
        {
            this.row = _row;
            this.col = _col;
            this.Image = isWhite ? new Bitmap(Properties.Resources.whitePawn) : new Bitmap(Properties.Resources.blackPawn);
            _ = isWhite ? this.IsWhite = true : this.IsBlack = true;
        }

        public override bool IsCanMove(ChessBoard Board, int toR, int toC, bool onlyCheck)
        {
            if (this.IsWhite)
            {
                //Проверяем, может ли пешка прыгнуть через клетку в начале хода
                if (!(hasMoved) && row == 1 && toR == 3 && col == toC)
                {
                    if (Board[toR, toC] == null && Board[toR - 1, toC] == null)
                    {
                        if (!onlyCheck)
                            isJump = true;
                        return true;
                    }
                }

                //Может ли пешка пройти на клетку вперед
                if (Board[toR, toC] == null && toR - row == 1 && col == toC)
                {
                    return true;
                }

                //Бьет ли пешка влево?
                if (toR == (row + 1) && toC == (col - 1) && Board[toR, toC] != null && Board[toR, toC].IsBlack)
                {
                    return true;
                }

                //Бьет ли пешка вправо?
                if (toR == (row + 1) && toC == (col + 1) && Board[toR, toC] != null && Board[toR, toC].IsBlack)
                {
                    return true;
                }

                //Проверяем взятие на проходе влево
                if (toR - row == 1 && col - toC == 1 && Board[toR, toC] == null && Board[toR - 1, toC] != null &&
                Board[toR - 1, toC] is Pawn &&
                Board[toR - 1, toC].IsBlack  &&
                Board[toR - 1, toC].IsJump())
                {
                    if (!onlyCheck)
                        Board[toR - 1, toC] = null;
                    return true;
                }

                //Проверяем взятие на проходе вправо
                if (toR - row == 1 && toC - col == 1 && Board[toR, toC] == null && Board[toR - 1, toC] != null &&
                Board[toR - 1, toC] is Pawn &&
                Board[toR - 1, toC].IsBlack &&
                Board[toR - 1, toC].IsJump())
                {
                    if (!onlyCheck)
                        Board[toR - 1, toC] = null;
                    return true;
                }

                return false;
            }
            else
            {
                //Проверяем, может ли пешка прыгнуть через клетку в начале хода
                if (!(hasMoved) && row == 6 && toR == 4 && col == toC)
                {
                    if (Board[toR, toC] == null && Board[toR + 1, toC] == null)
                    {
                        if (!onlyCheck)
                            isJump = true;
                        return true;
                    }
                }

                //Может ли пешка пройти на клетку вперед
                if (Board[toR, toC] == null && row - toR == 1 && col == toC)
                {
                    return true;
                }

                //Бьет ли пешка влево?
                if (toR == (row - 1) && toC == (col - 1) && Board[toR, toC] != null && Board[toR, toC].IsWhite)
                {
                    return true;
                }

                //Бьет ли пешка вправо?
                if (toR == (row - 1) && toC == (col + 1) && Board[toR, toC] != null && Board[toR, toC].IsWhite)
                {
                    return true;
                }

                //Проверяем взятие на проходе влево
                if (row - toR == 1 && col - toC == 1 && Board[toR, toC] == null && Board[toR + 1, toC] != null &&
                Board[toR + 1, toC] is Pawn &&
                Board[toR + 1, toC].IsWhite &&
                Board[toR + 1, toC].IsJump())
                {
                    if (!onlyCheck)
                        Board[toR + 1, toC] = null;
                    return true;
                }

                //Проверяем взятие на проходе вправо
                if (row - toR == 1 && toC - col == 1 && Board[toR, toC] == null && Board[toR + 1, toC] != null &&
                Board[toR + 1, toC] is Pawn &&
                Board[toR + 1, toC].IsWhite &&
                Board[toR + 1, toC].IsJump())
                {
                    if (!onlyCheck)
                        Board[toR + 1, toC] = null;
                    return true;
                }

                return false;
            }
        }

        public override bool IsJump() { return isJump; }
        public override void clearIsJump() { isJump = false; }

    }
}
