using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation.Results;

using PawnArt.Logic;

namespace PawnArt.Application
{
    public class MatchMakePoolSingleton
    {
        private readonly ConcurrentDictionary<Guid, WaitingPlayer> waitingPlayers;
        private const int AccaptableRankDiffrance = 400;
        public MatchMakePoolSingleton()
        {
            waitingPlayers = new ConcurrentDictionary<Guid, WaitingPlayer>();
        }
        public void Add(WaitingPlayer waitingPlayer)
        {
            waitingPlayers[waitingPlayer.Player.Id] = waitingPlayer;
        }
        public IEnumerable<MatchMakeResult> RefreshMatches()
        {
            while(waitingPlayers.Count >= 2)
            {
                var playerToMatch = waitingPlayers.First().Value;
                var match = waitingPlayers
                    .Values
                    .ToList()
                    .Skip(1)
                    .Where(p => Math.Abs(p.Player.Rank - playerToMatch.Player.Rank) < AccaptableRankDiffrance)
                    .Where(p => p.TimeConrolMs == playerToMatch.TimeConrolMs)
                    .OrderBy(p => Math.Abs(p.Player.Rank - playerToMatch.Player.Rank))
                    .FirstOrDefault();
                if(match is null)
                {
                    yield break;
                }
                waitingPlayers.TryRemove(playerToMatch.Player.Id, out _);
                waitingPlayers.TryRemove(match.Player.Id, out _);
                yield return new MatchMakeResult(playerToMatch.TimeConrolMs, playerToMatch.Player, match.Player); 
            }

        }
    }
}
            /*foreach(Player p )
            var result = new List<Tuple<Logic.Player, Logic.Player>>();
            if(waitingPlayers.Count < 2)
                return result;

            Logic.Player? currentPlayerInQueue = null;

            while(currentPlayerInQueue is null || waitingPlayers.Min(x => Math.Abs(x.Rank - currentPlayerInQueue.Rank) < AccaptableRankDiffrance))
            {
                if(waitingPlayers.TryDequeue(out currentPlayerInQueue))
                {
                    var playerFound = waitingPlayers.OrderBy(x => Math.Abs(x.Rank - currentPlayerInQueue.Rank)).First();
                    result.Add(new Tuple<Logic.Player, Logic.Player>(currentPlayerInQueue, playerFound));
                    
                }
                else
                {
                    break;
                }
            }
            return result;*/
        



/* : IMatchMakeQueueStradegy
    {
        private readonly IClientNotifier notifier;
        private readonly IMatchMakeContextRepository matchMakeContextRepository;

        private const int AccaptableRankDiffrance = 400;
        private const int AccaptableWaitingTimeMs = 8000;

        private const double RankPriorityOutOfOverallPiority = 0.25;
        private const double TimePriorityOutOfOverallPiority = 0.25;
        //increase for more aggrasive matchmaking standarts
        private const double RankPriorityCalculationMultiplier = 3;
        //increase for more aggrasive matchmaking standarts
        private const double TimePriorityCalculationMultiplier = 3;
        private const double NoMatchFoundProrityRate = 0.30;

        public MatchMakePoolSingleton(IClientNotifier notifier, IMatchMakeContextRepository matchMakeContextRepository)
        {
            this.notifier = notifier;
            this.matchMakeContextRepository = matchMakeContextRepository;
         
        }
        public async Task AddAsync(Player playerToAdd)
        {
            var playersToMatch = await matchMakeContextRepository.GetAllAsync();
            var result = new List<Tuple<Player, double>>();
            foreach(var matchmakeContext in playersToMatch)
            {
                result.Add(new Tuple<Player, double>(matchmakeContext.PlayerMatchMaking,
                    PriorityScoreOfMatch(playerToAdd, matchmakeContext)));
            }
            var best = result.OrderBy(x => x.Item2).FirstOrDefault();
            if(best is null || best.Item2 < NoMatchFoundProrityRate)
            {
                await matchMakeContextRepository.AddItemByIdAsync(new MatchMakeContext(playerToAdd, new DateTimeOffset(), new Guid()));
            }
            else
            {
                await Task.WhenAll(new List<Task>(){
                    notifier.NotifyClientAboutMatchMakingByIdAsync(playerToAdd.Id),
                    notifier.NotifyClientAboutMatchMakingByIdAsync(best.Item1.Id)});
            }
        }
        private double PriorityScoreOfMatch(Player newPlayerMatched, MatchMakeContext matchMakeContextOfPlayer2)
        {
            return PriorityScoreOfMatchByRanking(newPlayerMatched, matchMakeContextOfPlayer2) * RankPriorityOutOfOverallPiority +
                PriorityScoreOfMatchByTimeWaited(newPlayerMatched, matchMakeContextOfPlayer2) * TimePriorityOutOfOverallPiority; 
        }
        //https://www.desmos.com/calculator/w62uzmmdhb
        private double PriorityScoreOfMatchByRanking(Player newPlayerMatched, MatchMakeContext matchMakeContextOfPlayer2)
        {
            double diff = Math.Abs(newPlayerMatched.Rank - matchMakeContextOfPlayer2.PlayerMatchMaking.Rank);
            return Math.Pow(1 - ( diff / AccaptableRankDiffrance ), RankPriorityCalculationMultiplier);
        }
        //https://www.desmos.com/calculator/kg6cxgxqoj
        private double PriorityScoreOfMatchByTimeWaited(Player newPlayerMatched, MatchMakeContext matchMakeContextOfPlayer2)
        {
            double timeWaited = Math.Abs((DateTimeOffset.UtcNow - matchMakeContextOfPlayer2.MatchMakeRequastIssueTime).TotalMilliseconds);
            return Math.Pow(( timeWaited / AccaptableWaitingTimeMs ), TimePriorityCalculationMultiplier);
        }
    }*/