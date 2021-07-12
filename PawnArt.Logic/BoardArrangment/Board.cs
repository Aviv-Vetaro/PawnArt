using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PawnArt.Logic.BoardArrangment
{
    /// <summary>
    /// represents a chess board
    /// </summary>
    public class Board
    {
        public List<Position> previosPositions
        {
            get;
            set;
        }
        public Position currentPosition
        {
            get;
            set;
        }
        /// <summary>
        /// creates a new chess boards
        /// </summary>
        private Board()
        {

        }
        public static Board DefultePositionBoard()
        {
            Board b = new Board();
            b.currentPosition = Position.StartingPosition();
            b.previosPositions = new List<Position>();
            b.previosPositions.Add(b.currentPosition);
            return b;
        }
        /// <summary>
        /// get the squares in the board
        /// </summary>
        [NotMapped]
        public IReadOnlyList<Square> Squares
        {
            get
            {

                return currentPosition.SquaresInPosition;
            }
        }
        /// <summary>
        /// check wather the current position on the board is drawn
        /// </summary>
        public bool Drawn
        {
            get
            {
                return currentPosition.DrawnBy50MovesRule || DrawnByThreeFoldRep;
            }

        }
        /// <summary>
        /// check if the position is drawn becuse of the 3-fold repatation rule
        /// </summary>
        public bool DrawnByThreeFoldRep
        {
            get
            {
                return previosPositions.Count(pos => pos.Equals(currentPosition)) == 3;
            }
        }
        /// <summary>
        /// check if the position on board is lost for white 
        /// </summary>
        public bool WhiteIsMated
        {
            get
            {
                if(currentPosition.PlayerToPlayColor is PieceColor.White && ( !PossiblePlysForWhite().Any() ) && ( !Drawn ))
                    return true;
                return false;
            }
        }
        /// <summary>
        /// check if the position on board is lost for black 
        /// </summary>
        public bool BlackIsMated
        {
            get
            {
                if(currentPosition.PlayerToPlayColor is PieceColor.Black && ( !PossiblePlysForBlack().Any() ) && ( !Drawn ))
                    return true;
                return false;
            }
        }
        /// <summary>
        /// applies a ply made by the user on the position (plays the ply)
        /// </summary>
        /// <param name="plyMade">the plyn that was played</param>
        public void MakePly(Ply plyMade)
        {
            currentPosition = currentPosition.AfterPly(plyMade);
            previosPositions.Add(currentPosition);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>get all the plys that black can play in the current position. empty if its not the black players turn</returns>
        public IEnumerable<Ply> PossiblePlysForBlack()
        {
            return currentPosition.PossiblePlysForBlack();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>get all the plys that White can play in the current position. empty if its not the Whites players turn</returns>
        public IEnumerable<Ply> PossiblePlysForWhite()
        {
            return currentPosition.PossiblePlysForWhite();
        }


    }
}
