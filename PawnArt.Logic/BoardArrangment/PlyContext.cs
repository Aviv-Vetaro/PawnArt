using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PawnArt.Logic.BoardArrangment;

namespace PawnArt.Logic.BoardArrangment
{
    /// <summary>
    /// represnts a ply in a context of a game
    /// </summary>
    public record PlyContext(PieceColor PlayerColor, Ply PlyMade, DateTimeOffset Time);
}
