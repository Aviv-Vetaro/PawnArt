using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using PawnArt.Logic;

namespace PawnArt.Application.Player.GetPlayerById
{
    public class Executor : IQueryExecutor<GetPlayerByIdQuary, PawnArt.Logic.Player>
    {
        private readonly IPlayerRepository playerRepository;

        public Executor(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }
        public async Task<PawnArt.Logic.Player> ExecuteAsync(GetPlayerByIdQuary query, CancellationToken cancellationToken)
        {
            return await playerRepository.GetItemByIdAsync(query.Id);
        }
    }
}
