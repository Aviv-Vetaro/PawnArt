using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using PawnArt.Logic;

namespace PawnArt.Application.Player.RegisterNewPlayer
{
    public class Executor : ICommandExecutor<RegisterNewPlayerCommand>
    {
        private readonly IPlayerRepository playerRepository;
        private const int DefualtRank = 800;

        public Executor(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }
        public async Task ExecuteAsync(RegisterNewPlayerCommand command, CancellationToken cancellationToken)
        {
            Logic.Player newPlayer = new Logic.Player(command.Username, command.PlayerId, DefualtRank);
            await playerRepository.AddItemAsync(newPlayer);
        }
    }
}
