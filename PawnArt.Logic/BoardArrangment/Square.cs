using System;

namespace PawnArt.Logic.BoardArrangment
{
    /// <summary>
    /// represents a square in a board
    /// </summary>
    public record Square(Piece? Occupier, int Row, int File) : IComparable<Square>
    {
        [Obsolete("Contructor for EF Core use only", true)]
        private Square() : this(returnNull())
        {

        }

        private static Piece returnNull()
        {
            return null;
        }
        /// <summary>
        /// the row index of the square
        /// </summary>

        public int Row
        {
            get; set;
        } = Row;
        /// <summary>
        /// the file index of the square
        /// </summary>
        public int File
        {
            get; set;
        } = File;
        /// <summary>
        /// creates a new squre instance with a given occupier and a defult index
        /// </summary>
        /// <param name="occupier">the squres occupier</param>
        public Square(Piece? occupier) : this(occupier, 0, 0)
        {
            this.Occupier = occupier;
        }
        /// <summary>
        /// changed the squares index
        /// </summary>
        /// <param name="row">the row index</param>
        /// <param name="file">the file index</param>
        public void SetLocation(int row, int file)
        {
            Row = row;
            File = file;
        }

        public int CompareTo(Square? other)
        {
            if(other is null)
                return 1;
            int result = this.Row.CompareTo(other.Row);
            if(result != 0)
                return result;
            return this.File.CompareTo(other.File);
        }
    }
}