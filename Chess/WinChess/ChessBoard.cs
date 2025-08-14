using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinChess
{
    public class ChessBoard
    {
        private ChessPiece[,] board;

        public ChessPiece this[int r, int c]
        {
            get
            {
                return board[r, c];
            }
            set
            {
                board[r, c] = value;
            }
        }

        public bool whiteToMove = false;

        public bool whiteInCheck = false;
        public bool blackInCheck = false;

        internal void Initialize(bool isClassic, int? seed = null)
        {
            board = new ChessPiece[8, 8];
            whiteToMove = true;
            whiteInCheck = false;
            blackInCheck = false;
            if (isClassic) InitializeBoard();
            else Initialize960Board(seed);
        }

        private void InitializeBoard()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    board[r,c] = null;
                }
            }

            for (int i = 0; i < 8; i++)
            {
                board[1,i] = new Pieces.Pawn(1, i, true);
                board[6,i] = new Pieces.Pawn(6, i, false);
            }

            board[0,0] = new Pieces.Rook(0, 0, true);
            board[0,1] = new Pieces.Knight(0, 1, true);
            board[0,2] = new Pieces.Bishop(0, 2, true);
            board[0,3] = new Pieces.Queen(0, 3, true);
            board[0,4] = new Pieces.King(0, 4, true);
            board[0,5] = new Pieces.Bishop(0, 5, true);
            board[0,6] = new Pieces.Knight(0, 6, true);
            board[0,7] = new Pieces.Rook(0, 7, true);
            board[7,0] = new Pieces.Rook(7, 0, false);
            board[7,1] = new Pieces.Knight(7, 1, false);
            board[7,2] = new Pieces.Bishop(7, 2, false);
            board[7,3] = new Pieces.Queen(7, 3, false);
            board[7,4] = new Pieces.King(7, 4, false);
            board[7,5] = new Pieces.Bishop(7, 5, false);
            board[7,6] = new Pieces.Knight(7, 6, false);
            board[7,7] = new Pieces.Rook(7, 7, false);
        }

        private void Initialize960Board(int? seed)
        {
            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 8; c++)
                    board[r, c] = null;

            var rng = seed.HasValue ? new System.Random(seed.Value) : new System.Random();
            var back = Generate960BackRank(rng); // 8 chars: R/N/B/Q/K with bishops on opposite colors and K between Rs

            // White back rank on row 0
            for (int c = 0; c < 8; c++)
            {
                switch (back[c])
                {
                    case 'R': board[0, c] = new Pieces.Rook(0, c, true); break;
                    case 'N': board[0, c] = new Pieces.Knight(0, c, true); break;
                    case 'B': board[0, c] = new Pieces.Bishop(0, c, true); break;
                    case 'Q': board[0, c] = new Pieces.Queen(0, c, true); break;
                    case 'K': board[0, c] = new Pieces.King(0, c, true); break;
                }
            }

            // Pawns
            for (int i = 0; i < 8; i++)
            {
                board[1, i] = new Pieces.Pawn(1, i, true);
                board[6, i] = new Pieces.Pawn(6, i, false);
            }

            // Mirror for black on row 7 (same files, black pieces)
            for (int c = 0; c < 8; c++)
            {
                switch (back[c])
                {
                    case 'R': board[7, c] = new Pieces.Rook(7, c, false); break;
                    case 'N': board[7, c] = new Pieces.Knight(7, c, false); break;
                    case 'B': board[7, c] = new Pieces.Bishop(7, c, false); break;
                    case 'Q': board[7, c] = new Pieces.Queen(7, c, false); break;
                    case 'K': board[7, c] = new Pieces.King(7, c, false); break;
                }
            }
        }

        private char[] Generate960BackRank(System.Random rng)
        {
            var back = new char[8];
            var remaining = new System.Collections.Generic.List<int>(System.Linq.Enumerable.Range(0, 8));

            // 1) Bishops on opposite colors
            var dark = new[] { 0, 2, 4, 6 };
            var light = new[] { 1, 3, 5, 7 };
            int bDark = dark[rng.Next(dark.Length)];
            back[bDark] = 'B'; remaining.Remove(bDark);
            int bLight = light[rng.Next(light.Length)];
            back[bLight] = 'B'; remaining.Remove(bLight);

            // 2) Queen
            int q = remaining[rng.Next(remaining.Count)];
            back[q] = 'Q'; remaining.Remove(q);

            // 3) Two Knights
            int n1 = remaining[rng.Next(remaining.Count)];
            back[n1] = 'N'; remaining.Remove(n1);
            int n2 = remaining[rng.Next(remaining.Count)];
            back[n2] = 'N'; remaining.Remove(n2);

            // 4) Remaining three squares become R-K-R in file order so K lies between rooks
            remaining.Sort(); // a < b < c
            back[remaining[0]] = 'R';
            back[remaining[1]] = 'K';
            back[remaining[2]] = 'R';

            return back;
        }

        internal ChessPiece GetSelected()
        {
            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 8; c++)
                    if (board[r, c] != null && board[r, c].IsSelected)
                        return board[r, c];
            return null;
        }

        internal bool SetSelected(Point p)
        {
            ClearSelected();
            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 8; c++)
                    if (board[r, c] != null && board[r, c].Rect.Contains(p))
                    {
                        if ((whiteToMove && board[r, c].IsWhite) || (!whiteToMove && board[r, c].IsBlack))
                        {
                            board[r, c].IsSelected = true;
                            return true;
                        }
                        return false;
                    }
            return false;
        }

        internal void ClearSelected()
        {
            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 8; c++)
                    if (board[r, c] != null)
                        board[r, c].IsSelected = false;
        }

        internal void CalculateMoves(ChessPiece piece)
        {
            piece.CanMoves.Clear();
            piece.CanAttackes.Clear();

            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 8; c++)
                {
                    if (IsCanMove(piece.row, piece.col, r, c, true)) // We call the move check method with the onlyCheck flag enabled
                    {
                        if (board[r, c] == null)
                            piece.CanMoves.Add(r * 10 + c);
                        else
                            piece.CanAttackes.Add(r * 10 + c);
                    }
                }
        }

        public bool IsCanMove(int fromR, int fromC, int toR, int toC, bool onlyCheck)
        {
            if (fromR == toR && fromC == toC) return false;

            if (!board[fromR, fromC].IsCanMove(this, toR, toC, onlyCheck)) return false;

            
            

            ChessPiece whiteKing = null;
            ChessPiece blackKing = null;
            ChessPiece temp = board[toR, toC];

            // Let's get kings
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    if (board[r, c] is Pieces.King && board[r, c].IsWhite) whiteKing = board[r, c];
                    if (board[r, c] is Pieces.King && board[r, c].IsBlack) blackKing = board[r, c];
                }
            }

            // We check if the king is already under check and after this move it will also be under check, then we cannot move. We made a move that does not remove the king from check
            if (whiteKing.IsInCheck(this))
            {
                movePiece(fromR, fromC, toR, toC);
                if (whiteKing.IsInCheck(this))
                {
                    movePiece(toR, toC, fromR, fromC);
                    board[toR, toC] = temp;
                    return false;
                }
                movePiece(toR, toC, fromR, fromC);
                board[toR, toC] = temp;
            }

            if (blackKing.IsInCheck(this))
            {
                movePiece(fromR, fromC, toR, toC);
                if (blackKing.IsInCheck(this))
                {
                    movePiece(toR, toC, fromR, fromC);
                    board[toR, toC] = temp;
                    return false;
                }
                movePiece(toR, toC, fromR, fromC);
                board[toR, toC] = temp;
            }

            // We check that after the move, the king will be under check. We made a move that puts our king under check.
            movePiece(fromR, fromC, toR, toC);
            if (whiteToMove && whiteKing.IsInCheck(this))
            {
                movePiece(toR, toC, fromR, fromC);
                board[toR, toC] = temp;
                return false;
            }
            if (!whiteToMove && blackKing.IsInCheck(this))
            {
                movePiece(toR, toC, fromR, fromC);
                board[toR, toC] = temp;
                return false;
            }

            if (onlyCheck)
            {
                movePiece(toR, toC, fromR, fromC);
                board[toR, toC] = temp;
                return true;
            }

            // We put up the check flag
            if (whiteKing.IsInCheck(this))
            {
                whiteInCheck = true;
            }
            else whiteInCheck = false;
            if (blackKing.IsInCheck(this))
            {
                blackInCheck = true;
            }
            else blackInCheck = false;

            movePiece(toR, toC, fromR, fromC);
            board[toR, toC] = temp;

            whiteToMove = !whiteToMove;
            return true;
        }

        internal bool movePiece(int fromR, int fromC, int toR, int toC)
        {
            ChessPiece aPiece = board[fromR, fromC];
            aPiece.row = toR;
            aPiece.col = toC;
            board[toR, toC] = aPiece;
            bool promotion = IsPawnTransformation(fromR, fromC, toR, toC);
            board[fromR, fromC] = null;
            return promotion;
        }

        internal bool IsCheckmate()     // We get all possible moves
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    if (board[r, c] == null) continue;
                    if (whiteToMove && board[r, c].IsBlack) continue;
                    if (!whiteToMove && board[r, c].IsWhite) continue;
                    int[] possMoves = GetPossibleMoves(board[r, c]);
                    if (possMoves.Length > 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private int[] GetPossibleMoves(ChessPiece piece)
        {
            List<int> result = new();

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    if (IsCanMove(piece.row, piece.col, r, c, true))
                    {
                        result.Add((10 * r) + c);
                    }
                }
            }
            return result.ToArray();
        }

        internal void ClearIsJump() // Clearing the Pawn Jump Flag
        {
            // If the current move is white or black, then the opposite side has already made a move and the flag needs to be cleared, even if it wasn't used
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    if (board[r, c] == null) continue;
                    if (whiteToMove)
                    {
                        if (board[r, c].IsWhite) board[r, c].clearIsJump();
                    }
                    else
                    {
                        if (board[r, c].IsBlack) board[r, c].clearIsJump();
                    }
                }
            }
        }

        // We check if the pawn has reached the border, then we allow choosing a piece
        internal bool IsPawnTransformation(int fromR, int fromC, int toR, int toC) 
        {
            if (board[fromR, fromC] == null) return false;
            if (board[fromR, fromC] is Pieces.Pawn && board[fromR, fromC].IsBlack)
            {
                if (toR == 0) return true;
            }
            if (board[fromR, fromC] is Pieces.Pawn && board[fromR, fromC].IsWhite)
            {
                if (toR == 7) return true;
            }
            return false;
        }

        internal void PromotePawnAt(int r, int c, WinChess.PromotionChoice choice)
        {
            var p = this[r, c];
            if (p == null) return;

            bool isWhite = p is Pieces.Pawn && p.IsWhite;

            // Replace the pawn with the chosen piece at the same square
            switch (choice)
            {
                case WinChess.PromotionChoice.Queen:
                    this[r, c] = isWhite ? new Pieces.Queen(r, c, true) : new Pieces.Queen(r, c, false);
                    break;
                case WinChess.PromotionChoice.Rook:
                    this[r, c] = isWhite ? new Pieces.Rook(r, c, true) : new Pieces.Rook(r, c, false);
                    break;
                case WinChess.PromotionChoice.Bishop:
                    this[r, c] = isWhite ? new Pieces.Bishop(r, c, true) : new Pieces.Bishop(r, c, false);
                    break;
                case WinChess.PromotionChoice.Knight:
                    this[r, c] = isWhite ? new Pieces.Knight(r, c, true) : new Pieces.Knight(r, c, false);
                    break;
            }
        }
    }
}
