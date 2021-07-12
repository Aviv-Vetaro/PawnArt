using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using PawnArt.DataAccess.Microsoft.EntityFrameworkCore;
using PawnArt.Logic;
using PawnArt.Logic.DataAccessInterface;

namespace PawnArt.DataAccess
{
    public class GameRepository : IGameRepository
    {
        private readonly GameContext gameContext;

        public GameRepository(GameContext gameContext)
        {
            this.gameContext = gameContext;
        }
        public async Task AddItemAsync(Game item)
        {
            await Task.FromResult(gameContext.Games.Add(item));
        }

        public async Task DeleteItemByIdAsync(Guid id)
        {
            var game = await GetItemByIdAsync(id);
            gameContext.Games.Remove(game);
        }

        public async Task<IEnumerable<Game>> GetGamesByPlayerId(Guid playerId)
        {
            return await gameContext.Games.Include(gameContext.GetIncludePaths(typeof(Game))).Where(g => g.BlackPlayer.Id == playerId || g.WhitePlayer.Id == playerId).ToListAsync();
        }

        public async Task<Game> GetItemByIdAsync(Guid id)
        {
            //return await gameContext.Games.Include(g => g.Board).ThenInclude(b => b.CurrentPosition.Squares).ThenInclude(b => b).SingleOrDefaultAsync(g => g.Id == id));

            try
            {
                var paths = gameContext.GetIncludePaths(typeof(Game));
                var quary = gameContext.Games.Include(paths)
                    /*
                    .Include(g => g.Board.currentPosition.squares)
                    .ThenInclude(s => s.Row)

                    .Include(g => g.Board.currentPosition.squares)
                    .ThenInclude(s => s.File)

                    .Include(g => g.Board.currentPosition.squares)
                    .ThenInclude(s => s.Occupier)
                    */
                    .Include(g => g.Board.currentPosition.squares)
                    .ThenInclude(s => s.Occupier.Color)

                    .Include(g => g.Board.currentPosition.squares)
                    .ThenInclude(s => s.Occupier.Type)

                    .Include(g => g.Board.currentPosition.squares)
                    .ThenInclude(s => s.Occupier.Moved)



                    .Include(g => g.Board.previosPositions)
                    .ThenInclude(p => p.squares)
                    .ThenInclude(s => s.Row)

                    .Include(g => g.Board.previosPositions)
                    .ThenInclude(p => p.squares)
                    .ThenInclude(s => s.File)

                    .Include(g => g.Board.previosPositions)
                    .ThenInclude(p => p.squares)
                    .ThenInclude(s => s.Occupier)

                    .Include(g => g.Board.previosPositions)
                    .ThenInclude(p => p.squares)
                    .ThenInclude(s => s.Occupier.Color)

                    .Include(g => g.Board.previosPositions)
                    .ThenInclude(p => p.squares)
                    .ThenInclude(s => s.Occupier.Type)

                    .Include(g => g.Board.previosPositions)
                    .ThenInclude(p => p.squares)
                    .ThenInclude(s => s.Occupier.Moved)

                    .Include("Board.currentPosition.canWhiteCastleKingSide")
                    .Include("Board.currentPosition.canWhiteCastleQueenSide")
                    .Include("Board.currentPosition.canBlackCastleKingSide")
                    .Include("Board.currentPosition.canBlackCastleQueenSide")
                    .Include("Board.currentPosition.enPassentTakablePawnOfWhite")
                    .Include("Board.currentPosition.enPassentTakablePawnOfBlack")

                    .Include("Board.previosPositions.canWhiteCastleKingSide")
                    .Include("Board.previosPositions.canWhiteCastleQueenSide")
                    .Include("Board.previosPositions.canBlackCastleKingSide")
                    .Include("Board.previosPositions.canBlackCastleQueenSide")
                    .Include("Board.previosPositions.enPassentTakablePawnOfWhite")
                    .Include("Board.previosPositions.enPassentTakablePawnOfBlack")

                    .Include("Board.state.whitePlayerTime")
                    .Include("Board.state.blackPlayerTime")
                    .Include("Board.state.lastPlyTime")
                    .AsSplitQuery();
                return await quary.SingleOrDefaultAsync(g => g.Id == id);
            }
            catch(Exception e)
            {
                throw;
            }

        }
    }
}
