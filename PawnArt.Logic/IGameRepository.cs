using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnArt.Logic.DataAccessInterface
{
    public interface IGameRepository : IRepository<Game>  
    {
        Task<IEnumerable<Game>> GetGamesByPlayerId(Guid playerId);
    }
}
