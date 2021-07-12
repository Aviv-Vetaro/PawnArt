using System;

namespace PawnArt.Logic.BoardArrangment
{
    /// <summary>
    /// represents a ply (a.k.a a move) in a chess board
    /// </summary>
    public record Ply(Square Source, Square Desitination, PieceType? PiecePromoted = null)
    {
        [Obsolete("Contructor for EF Core use only", true)]
        public Ply() : this(null, null)
        {

        }
        /// <summary>
        /// The piece thats moves in the ply
        /// </summary>
        public Piece Piece
        {
            get
            {
                return Source.Occupier;
            }
            init
            {
                Source = Source with
                {
                    Occupier = value
                };
            }
        }
        public readonly static Ply WhiteQueenSideCastle = new Ply
        (
            new Square(new Piece(PieceType.King, PieceColor.White), 4, 0),
            new Square(new Piece(PieceType.Rook, PieceColor.White), 2, 0)
        );
        public readonly static Ply WhiteKingSideCastle = new Ply
        (
            new Square(new Piece(PieceType.King, PieceColor.White), 4, 0),
            new Square(new Piece(PieceType.Rook, PieceColor.White), 6, 0)
        );
        public readonly static Ply BlackQueenSideCastle = new Ply
        (
            new Square(new Piece(PieceType.King, PieceColor.Black), 4, 7),
            new Square(new Piece(PieceType.Rook, PieceColor.Black), 2, 7)
        );
        public readonly static Ply BlackKingSideCastle = new Ply
        (
            new Square(new Piece(PieceType.King, PieceColor.Black), 4, 7),
            new Square(new Piece(PieceType.Rook, PieceColor.Black), 6, 7)
        );

    }
}
