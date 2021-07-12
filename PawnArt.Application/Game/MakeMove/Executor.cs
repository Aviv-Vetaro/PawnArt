using System;
using System.Threading;
using System.Threading.Tasks;

using PawnArt.Logic;
using PawnArt.Logic.BoardArrangment;
using PawnArt.Logic.DataAccessInterface;

namespace PawnArt.Application.Game.MakeMove
{
    public class Executor : ICommandExecutor<MakeMoveCommand>
    {
        private readonly IGameRepository gameRepository;
        private readonly IPlayerRepository playerRepository;

        public Executor(IGameRepository gameRepository, IPlayerRepository playerRepository)
        {
            this.gameRepository = gameRepository;
            this.playerRepository = playerRepository;
        }
        public async Task ExecuteAsync(MakeMoveCommand command, CancellationToken cancellationToken)
        {
            Logic.Game game = await gameRepository.GetItemByIdAsync(command.GameId);
            Logic.BoardArrangment.PieceColor colorOfPlayer = ( await gameRepository.GetItemByIdAsync(command.GameId) ).WhitePlayer.Id.Equals(command.PlayerThatMadeTheMoveId) ? PieceColor.White : PieceColor.Black;
            var timeNow = DateTimeOffset.UtcNow;
            game.MakePly(new PlyContext(colorOfPlayer, command.MoveMade, timeNow));
            /*command.
            if(game.WhitePlayer.Id == command.playerThatMadeTheMoveId)
            {
                game.MakePly()
            }
            */
        }
    }
}
