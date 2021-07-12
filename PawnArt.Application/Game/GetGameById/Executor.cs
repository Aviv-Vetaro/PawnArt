using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using PawnArt.Logic.DataAccessInterface;

namespace PawnArt.Application.Game.GetGameById
{
    public class Executor : IQueryExecutor<GetGameByIdQuary, Logic.Game>
    {
        private readonly IGameRepository gameRepository;

        public Executor(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }
        public async Task<Logic.Game> ExecuteAsync(GetGameByIdQuary query, CancellationToken cancellationToken)
        {
            return await gameRepository.GetItemByIdAsync(query.Id);
        }
    }
}
