using System;

using PawnArt.Logic.BoardArrangment;

namespace PawnArt.Application.Game.MakeMove
{
    public record MakeMoveCommand(Guid GameId, Guid PlayerThatMadeTheMoveId, Ply MoveMade) : ICommand
    {
    }
}
