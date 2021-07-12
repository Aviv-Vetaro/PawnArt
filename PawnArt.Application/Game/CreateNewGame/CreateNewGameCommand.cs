using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnArt.Application.Game.CreateNewGame
{
    public record CreateNewGameCommand(Guid WhitePlayerId, Guid BlackPlayerId, TimeSpan TimeControl) : ICommand<Guid>;
}
