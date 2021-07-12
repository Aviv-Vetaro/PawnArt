using System;

using PawnArt.Logic.BoardArrangment;

namespace PawnArt.Logic
{
    /// <summary>
    /// represents a state of a chess clock in a game 
    /// </summary>
    public record ClockState
    {
        private TimeSpan whitePlayerTime;
        private TimeSpan blackPlayerTime;
        private DateTimeOffset lastPlyTime;
        /// <summary>
        /// check if whites time ran out
        /// </summary>
        public bool WhiteFlagFall
        {
            get
            {
                return whitePlayerTime.TotalMilliseconds <= 0;
            }

        }
        /// <summary>
        /// check if blacks time ran out
        /// </summary>
        public bool BlackFlagFall
        {
            get
            {
                return blackPlayerTime.TotalMilliseconds <= 0;
            }
        }

        /*
        public long LastPlyTimeinTicks
        {
            get
            {
                return lastPlyTime.Ticks;
            }
            set
            {
                lastPlyTime = new DateTimeOffset(value, new TimeSpan());
            }
        }
        */
        /// <summary>
        /// creates a new ClockState instance
        /// </summary>
        /// <param name="gameStart">the time the game started in</param>
        /// <param name="timeForEachPlayer">the time allocated for each player</param>
        public ClockState(DateTimeOffset gameStart, TimeSpan timeForEachPlayer)
        {
            lastPlyTime = gameStart;
            whitePlayerTime = timeForEachPlayer;
            blackPlayerTime = timeForEachPlayer;
        }
        [Obsolete("for EF Core use only", true)]
        private ClockState()
        {

        }
        private ClockState(DateTimeOffset lastPlyTime, TimeSpan whitePlayerTime, TimeSpan blackPlayerTime)
        {
            this.lastPlyTime = lastPlyTime.UtcDateTime;
            this.blackPlayerTime = blackPlayerTime;
            this.whitePlayerTime = whitePlayerTime;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ply">the context of the ply made</param>
        /// <returns>the state after the ply</returns>
        public ClockState After(PlyContext ply)
        {
            //if(!IsWithinTimeLimits(ply))
            //    throw new Exception("Probably should be a custom exception.");

            var timeLeftForCurrentPlayer = ( ply.PlayerColor is PieceColor.White ? whitePlayerTime : blackPlayerTime ) - ( ply.Time - lastPlyTime );
            if(ply.PlayerColor is PieceColor.White)
            {
                whitePlayerTime = timeLeftForCurrentPlayer;
            }
            else
            {
                blackPlayerTime = timeLeftForCurrentPlayer;
            }

            return new ClockState(ply.Time, whitePlayerTime, blackPlayerTime);
        }

        /*private bool IsWithinTimeLimits(PlyContext ply)
        {
            return timeLeft[ply.PlayerColor] >= ( ply.Time - lastPlyTime );
        }*/
    }
}
