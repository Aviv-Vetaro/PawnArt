
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PawnArt.Tests
{
    [TestClass]
    public class GameMoveGenerationTester
    {
        /*
        [TestMethod]
        public void TestStartPositionMovesForWhite()
        {
            Board board = new Board();

            var plys = board.PossiblePlysForWhite().ToList();

            var expectedResult = new List<Ply>();

            for(int i = 0; i < 8; i++)
            {
                expectedResult.Add(new Ply(
                    new Square(new Piece(PieceType.Pawn, PieceColor.White), 1, i),
                    new Square(null, 2, i)));
                expectedResult.Add(new Ply(
                    new Square(new Piece(PieceType.Pawn, PieceColor.White), 1, i),
                    new Square(null, 3, i)));
            }
            expectedResult.Add(new Ply(
                    new Square(new Piece(PieceType.Knight, PieceColor.White), 0, 1),
                    new Square(null, 2, 0)));
            expectedResult.Add(new Ply(
                    new Square(new Piece(PieceType.Knight, PieceColor.White), 0, 1),
                    new Square(null, 2, 2)));

            expectedResult.Add(new Ply(
                new Square(new Piece(PieceType.Knight, PieceColor.White), 0, 6),
                new Square(null, 2, 7)));
            expectedResult.Add(new Ply(
                    new Square(new Piece(PieceType.Knight, PieceColor.White), 0, 6),
                    new Square(null, 2, 5)));
            CollectionAssert.AreEqual(expectedResult, plys);
        }*/
    }
}
