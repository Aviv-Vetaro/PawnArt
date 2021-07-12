using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PawnArt.Logic;
namespace PawnArt.Application.Player.GetPlayerById
{
    public record GetPlayerByIdQuary(Guid Id) : IQuery<PawnArt.Logic.Player>;
}
