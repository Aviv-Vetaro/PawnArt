using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnArt.Logic
{
    public class Player : IEquatable<Player?>
    {
        public Player(string userName, Guid id, int rank)
        {
            this.UserName = userName;
            this.Id = id;
            this.Rank = rank;
        }

        public string UserName
        {
            get;
        }
        public Guid Id
        {
            get;
        }
        public int Rank
        {
            get;
            private set;
        }

        public override bool Equals(object? obj)
        {
            return this.Equals(obj as Player);
        }

        public bool Equals(Player? other)
        {
            return other != null &&
                    this.Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id);
        }

        public static bool operator ==(Player? left, Player? right)
        {
            return EqualityComparer<Player>.Default.Equals(left, right);
        }

        public static bool operator !=(Player? left, Player? right)
        {
            return !( left == right );
        }
    }
}
