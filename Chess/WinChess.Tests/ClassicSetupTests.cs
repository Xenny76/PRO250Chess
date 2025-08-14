using WinChess;
using Xunit;

namespace WinChess.Tests
{
    public class ClassicSetupTests
    {
        [Fact]
        public void WhiteToMove_And_NoChecks_OnStart()
        {
            var b = new ChessBoard();
            b.Initialize(true);

            Assert.True(b.whiteToMove);
            Assert.False(b.whiteInCheck);
            Assert.False(b.blackInCheck);
        }

        [Fact]
        public void Classic_Pawns_AreOnRows1And6()
        {
            var b = new ChessBoard();
            b.Initialize(true);

            for (int c = 0; c < 8; c++)
            {
                Assert.True(b[1, c] is Pieces.Pawn && b[1, c].IsWhite);
                Assert.True(b[6, c] is Pieces.Pawn && b[6, c].IsBlack);
            }
        }

        [Fact]
        public void Classic_WhiteBackRank_IsStandard()
        {
            var b = new ChessBoard();
            b.Initialize(true);

            Assert.True(b[0, 0] is Pieces.Rook && b[0, 0].IsWhite);
            Assert.True(b[0, 1] is Pieces.Knight && b[0, 1].IsWhite);
            Assert.True(b[0, 2] is Pieces.Bishop && b[0, 2].IsWhite);
            Assert.True(b[0, 3] is Pieces.Queen && b[0, 3].IsWhite);
            Assert.True(b[0, 4] is Pieces.King && b[0, 4].IsWhite);
            Assert.True(b[0, 5] is Pieces.Bishop && b[0, 5].IsWhite);
            Assert.True(b[0, 6] is Pieces.Knight && b[0, 6].IsWhite);
            Assert.True(b[0, 7] is Pieces.Rook && b[0, 7].IsWhite);
        }

        [Fact]
        public void Classic_BlackBackRank_IsStandard()
        {
            var b = new ChessBoard();
            b.Initialize(true);

            Assert.True(b[7, 0] is Pieces.Rook && b[7, 0].IsBlack);
            Assert.True(b[7, 1] is Pieces.Knight && b[7, 1].IsBlack);
            Assert.True(b[7, 2] is Pieces.Bishop && b[7, 2].IsBlack);
            Assert.True(b[7, 3] is Pieces.Queen && b[7, 3].IsBlack);
            Assert.True(b[7, 4] is Pieces.King && b[7, 4].IsBlack);
            Assert.True(b[7, 5] is Pieces.Bishop && b[7, 5].IsBlack);
            Assert.True(b[7, 6] is Pieces.Knight && b[7, 6].IsBlack);
            Assert.True(b[7, 7] is Pieces.Rook && b[7, 7].IsBlack);
        }
    }
}
