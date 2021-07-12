using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnArt.Application.Game.GetGameById
{
    public record GetGameByIdQuary(Guid Id) : IQuery<Logic.Game>
    {
    }
}
