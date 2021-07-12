using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnArt.Application
{
    public record WaitingPlayer(Logic.Player Player, int TimeConrolMs);
}
