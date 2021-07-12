using System;

namespace PawnArt.Logic.BoardArrangment
{

    /// <summary>
    /// represents a chess piece
    /// </summary>
    public record Piece(PieceType Type, PieceColor Color, bool Moved = false)
    {
        [Obsolete("Contructor for EF Core use only", true)]
        public Piece() : this(PieceType.Pawn, PieceColor.White)
        {

        }
    }
}