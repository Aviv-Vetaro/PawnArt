using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using PawnArt.Logic;

namespace PawnArt.DataAccess
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly GameContext gameContext;

        public PlayerRepository(GameContext gameContext)
        {
            this.gameContext = gameContext;
        }
        public async Task AddItemAsync(Player item)
        {
            await gameContext.Players.AddAsync(item);
        }

        public async Task DeleteItemByIdAsync(Guid id)
        {
            var playerToDelete = await GetItemByIdAsync(id);
            gameContext.Players.Remove(playerToDelete);
        }

        public async Task<Player> GetItemByIdAsync(Guid id)
        {
            return await gameContext.Players.SingleOrDefaultAsync(p => p.Id == id);
        }

    }
}
