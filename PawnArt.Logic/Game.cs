using System;
using System.Collections.Generic;

using PawnArt.Logic.BoardArrangment;

namespace PawnArt.Logic
{
    public class Game
    {
        private ClockState state;
        /// <summary>
        /// the underlying board of the game
        /// </summary>
        public Board Board
        {
            get;
            private set;
        }

        //private readonly IDictionary<PieceColor, Player> playerRoles;
        private Game(Guid id)
        {
            this.Id = id;
        }
        /// <summary>
        /// creates a new game
        /// </summary>
        /// <param name="id">the game id</param>
        /// <param name="whitePlayer">the white player in the game</param>
        /// <param name="blackPlayer">the black player in the game</param>
        /// <param name="timeStarted">the time the game started</param>
        /// <param name="timeControl">the time allocated for each player</param>
        public Game(Guid id, Player whitePlayer, Player blackPlayer, DateTimeOffset timeStarted, TimeSpan timeControl)
        {
            Board = Board.DefultePositionBoard();
            this.WhitePlayer = whitePlayer;
            this.BlackPlayer = blackPlayer;
            state = new ClockState(timeStarted, timeControl);
            this.Id = id;
        }
        /// <summary>
        /// the squres in the game's board
        /// </summary>
        public IReadOnlyList<Square> Squares
        {
            get
            {
                return Board.Squares;
            }
        }
        /// <summary>
        /// the white player in the game
        /// </summary>
        public Player WhitePlayer
        {
            get;
            private set;
        }
        /// <summary>
        /// the black player in the game
        /// </summary>
        public Player BlackPlayer
        {
            get;
            private set;
        }
        /// <summary>
        /// weather the game is recurring or not
        /// </summary>
        public bool Recurring
        {
            get; set;
        }
        /// <summary>
        /// makes a ply in the game
        /// </summary>
        /// <param name="ply"></param>
        public void MakePly(PlyContext ply)
        {
            Board.MakePly(ply.PlyMade);
            state = state.After(ply);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>all possible moves for black</returns>
        public IEnumerable<Ply> PossiblePlysForBlack()
        {
            return Board.PossiblePlysForBlack();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>all possible moves for white</returns>
        public IEnumerable<Ply> PossiblePlysForWhite()
        {
            return Board.PossiblePlysForWhite();
        }
        public bool WhiteWon
        {
            get
            {
                return Board.BlackIsMated || state.WhiteFlagFall;
            }
        }
        public bool BlackWon
        {
            get
            {
                return Board.WhiteIsMated || state.BlackFlagFall;
            }
        }
        public bool Draw
        {
            get
            {
                return Board.Drawn;
            }
        }

        public Guid Id
        {
            get;
        }
    }
}




/*private readonly IBoardOperetor boardOperetor;
       private readonly IMessageSender sender;
       private bool whiteResinged;
       private bool blackResinged;
       private bool drawByAgreement;
       private List<IChessMove> whiteMoves;
       private List<IChessMove> blackMoves;
       private List<IChessPosition> movesSoFar;
       private List<Message> whitePlayerEntries;
       private List<Message> blackPlayerEntries;
       private ClockState clockState;
       public IChessPosition CurrentPosition
       {
           private set;
           get;
       }
       public Guid Id
       {
           get;
       }
       public Player WhitePlayer
       {
           get;
       }
       public Player BlackPlayer
       {
           get;
       }

       public bool IsWhitesTurn
       {
           private set;
           get;
       }
       public long WhiteTimeControlMs
       {
           get
           {
               return chessClock.WhiteTimeControlMs;
           }
       }
       public long BlackTimeControlMs
       {
           get
           {
               return chessClock.BlackTimeControlMs;
           }
       }
       public IEnumerable<IChessMove> WhiteMoves
       {
           get
           {
               return whiteMoves;
           }
       }

       public IEnumerable<IChessMove> BlackMoves
       {
           get
           {
               return blackMoves;
           }
       }


       public Game(Guid id, IBoardOperetor boardOperetor, Player whitePlayer, Player blackPlayer, DateTimeOffset startTime, TimeSpan timeForEachPlayer, IMessageSender sender) 
       {
           whitePlayerEntries = new List<Message>();
           blackPlayerEntries = new List<Message>();
           clockState = new ClockState(startTime, timeForEachPlayer);
           WhitePlayer = whitePlayer;
           BlackPlayer = blackPlayer;
           this.sender = sender;
           whiteMoves = new List<IChessMove>();
           blackMoves = new List<IChessMove>();
           this.boardOperetor = boardOperetor;
           IsWhitesTurn = true;
           whiteResinged = false;
           blackResinged = false;
           drawByAgreement = false;
           movesSoFar = new List<IChessPosition>();
           Id = id;
       }


       public void MakePly(Ply plyMade)
       {
           clockState = clockState.After(plyMade);
           CurrentPosition = boardOperetor.PreformChessMoveOnPosition(CurrentPosition, nextMove);
           IsWhitesTurn = !IsWhitesTurn;
       }
       public bool WhiteWon
       {
           get
           {
               return chessClock.HasBlackFlagFall() || ( boardOperetor.IsMate(CurrentPosition) && ( !IsWhitesTurn ) ) || blackResinged;
           }
       }
       public bool BlackWon
       {
           get
           {
               return chessClock.HasWhiteFlagFall() || ( boardOperetor.IsMate(CurrentPosition) && ( IsWhitesTurn ) ) || whiteResinged;
           }
       }
       public bool IsDraw
       {
           get
           {
               return HasThreefoldRep() || drawByAgreement;
           }
       }
       public bool IsGameOver
       {
           get
           {
               if(IsDraw || WhiteWon || BlackWon)
                   return true;
               return false;
           }
       }

       public void EndByAgreementDraw()
       {
           drawByAgreement = true;
       }
       public void EndByWhiteResegnition()
       {
           whiteResinged = true;
       }
       public void EndByBlackResegnition()
       {
           blackResinged = true;
       }

       private bool HasThreefoldRep()
       {
           if(movesSoFar.Where(x => x.Equals(movesSoFar.Last())).Count() >= 3)
               return true;
           return false;
       }

       public void SendMessageAsWhite(Message message)
       {
           MessageInfo messageInfo = new MessageInfo(message, WhitePlayer, BlackPlayer);
           sender.Send(messageInfo);
           whitePlayerEntries.Add(message);

       }
       public void SendMessageAsBlack(Message message)
       {
           MessageInfo messageInfo = new MessageInfo(message, BlackPlayer, WhitePlayer);
           sender.Send(messageInfo);
           whitePlayerEntries.Add(message);
       }*/