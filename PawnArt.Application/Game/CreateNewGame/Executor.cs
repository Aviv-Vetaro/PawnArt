using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PawnArt.Logic;
using PawnArt.Logic.DataAccessInterface;

namespace PawnArt.Application.Game.CreateNewGame
{
	internal class Executor : ICommandExecutor<CreateNewGameCommand, Guid>
	{
		private readonly IGameRepository gameRepository;
        private readonly IPlayerRepository playerRepository;

        public Executor(IGameRepository gameRepository, IPlayerRepository playerRepository)
		{
			this.gameRepository = gameRepository;
            this.playerRepository = playerRepository;
        }

        public async Task<Guid> ExecuteAsync(CreateNewGameCommand command, CancellationToken cancellationToken)
        {
			var whitePlayer = await playerRepository.GetItemByIdAsync(command.WhitePlayerId);
			var blackPlayer = await playerRepository.GetItemByIdAsync(command.BlackPlayerId);
			var newGame = new PawnArt.Logic.Game(new Guid(), whitePlayer, blackPlayer, new DateTimeOffset(), command.TimeControl);
			await gameRepository.AddItemAsync(newGame);
			return newGame.Id;
		}
    }
}
