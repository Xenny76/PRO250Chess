using System;
using System.Linq;
using WinChess;
using Xunit;

namespace WinChess.Tests
{
    public class Chess960SetupTests
    {
        [Fact]
        public void Chess960_Pawns_AreOnRows1And6()
        {
            var b = new ChessBoard();
            b.Initialize(false, seed: 42);

            for (int c = 0; c < 8; c++)
            {
                Assert.True(b[1, c] is Pieces.Pawn && b[1, c].IsWhite);
                Assert.True(b[6, c] is Pieces.Pawn && b[6, c].IsBlack);
            }
        }

        [Fact]
        public void Chess960_Bishops_OnOppositeColors_ForEachSide()
        {
            var b = new ChessBoard();
            b.Initialize(false, seed: 123);

            // White bishops on row 0
            var wbFiles = Enumerable.Range(0, 8)
                .Where(c => b[0, c] is Pieces.Bishop && b[0, c].IsWhite)
                .ToArray();
            Assert.Equal(2, wbFiles.Length);
            Assert.NotEqual((0 + wbFiles[0]) % 2, (0 + wbFiles[1]) % 2); // opposite colors

            // Black bishops on row 7
            var bbFiles = Enumerable.Range(0, 8)
                .Where(c => b[7, c] is Pieces.Bishop && b[7, c].IsBlack)
                .ToArray();
            Assert.Equal(2, bbFiles.Length);
            Assert.NotEqual((7 + bbFiles[0]) % 2, (7 + bbFiles[1]) % 2); // opposite colors
        }

        [Fact]
        public void Chess960_King_IsBetween_Rooks_ForEachSide()
        {
            var b = new ChessBoard();
            b.Initialize(false, seed: 999);

            static void AssertBetween(ChessBoard board, int row)
            {
                int k = Enumerable.Range(0, 8).Single(c => (row == 0 ? board[0, c].IsWhite : board[7, c].IsBlack) && (board[row, c] is Pieces.King));
                var rooks = Enumerable.Range(0, 8).Where(c => board[row, c] is Pieces.Rook).OrderBy(c => c).ToArray();
                Assert.Equal(2, rooks.Length);
                Assert.True(rooks[0] < k && k < rooks[1]);
            }

            AssertBetween(b, 0); // white
            AssertBetween(b, 7); // black
        }

        [Fact]
        public void Chess960_BackRanks_MirrorByType()
        {
            var b = new ChessBoard();
            b.Initialize(false, seed: 777);

            static char ToTypeChar(ChessPiece p)
            {
                if (p is Pieces.King) return 'K';
                if (p is Pieces.Queen) return 'Q';
                if (p is Pieces.Rook) return 'R';
                if (p is Pieces.Knight) return 'N';
                if (p is Pieces.Bishop) return 'B';
                return '.';
            }

            var whiteRow = new string(Enumerable.Range(0, 8).Select(c => ToTypeChar(b[0, c])).ToArray());
            var blackRow = new string(Enumerable.Range(0, 8).Select(c => ToTypeChar(b[7, c])).ToArray());

            Assert.Equal(whiteRow, blackRow);
        }

        [Fact]
        public void Chess960_Deterministic_WithSeed()
        {
            var b1 = new ChessBoard();
            var b2 = new ChessBoard();
            b1.Initialize(false, seed: 12345);
            b2.Initialize(false, seed: 12345);

            string Layout(ChessBoard board, int row) =>
                string.Concat(Enumerable.Range(0, 8).Select(c =>
                {
                    var p = board[row, c];
                    return p switch
                    {
                        Pieces.King => 'K',
                        Pieces.Queen => 'Q',
                        Pieces.Rook => 'R',
                        Pieces.Knight => 'N',
                        Pieces.Bishop => 'B',
                        _ => '.',
                    };
                }));

            Assert.Equal(Layout(b1, 0), Layout(b2, 0));
            Assert.Equal(Layout(b1, 7), Layout(b2, 7));
        }

        [Fact]
        public void Chess960_BackRankPieceCounts_AreCorrect()
        {
            var b = new ChessBoard();
            b.Initialize(false, seed: 2025);

            int CountAt<T>(int row) => Enumerable.Range(0, 8).Count(c => b[row, c] is T);

            // Per side: 1 King, 1 Queen, 2 Rooks, 2 Knights, 2 Bishops
            Assert.Equal(1, Enumerable.Range(0, 8).Count(c => b[0, c] is Pieces.King && b[0, c].IsWhite));
            Assert.Equal(1, Enumerable.Range(0, 8).Count(c => b[0, c] is Pieces.Queen && b[0, c].IsWhite));
            Assert.Equal(2, Enumerable.Range(0, 8).Count(c => b[0, c] is Pieces.Rook && b[0, c].IsWhite));
            Assert.Equal(2, Enumerable.Range(0, 8).Count(c => b[0, c] is Pieces.Knight && b[0, c].IsWhite));
            Assert.Equal(2, Enumerable.Range(0, 8).Count(c => b[0, c] is Pieces.Bishop && b[0, c].IsWhite));

            Assert.Equal(1, Enumerable.Range(0, 8).Count(c => b[7, c] is Pieces.King && b[7, c].IsBlack));
            Assert.Equal(1, Enumerable.Range(0, 8).Count(c => b[7, c] is Pieces.Queen && b[7, c].IsBlack));
            Assert.Equal(2, Enumerable.Range(0, 8).Count(c => b[7, c] is Pieces.Rook && b[7, c].IsBlack));
            Assert.Equal(2, Enumerable.Range(0, 8).Count(c => b[7, c] is Pieces.Knight && b[7, c].IsBlack));
            Assert.Equal(2, Enumerable.Range(0, 8).Count(c => b[7, c] is Pieces.Bishop && b[7, c].IsBlack));
        }
    }
}
