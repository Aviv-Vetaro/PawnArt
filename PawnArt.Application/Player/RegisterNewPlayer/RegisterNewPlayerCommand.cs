using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnArt.Application.Player.RegisterNewPlayer
{
    public record RegisterNewPlayerCommand(Guid PlayerId, string Username) : ICommand
    {
    }
}
