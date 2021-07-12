using System.Reflection;

using Microsoft.EntityFrameworkCore;

using PawnArt.Logic;
using PawnArt.Logic.BoardArrangment;

namespace PawnArt.DataAccess
{
    public class GameContext : DbContext
    {
        public GameContext(DbContextOptions<GameContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Game> Games
        {
            get; set;
        }
        public DbSet<Position> Positions
        {
            get; set;
        }
        public DbSet<Square> Squares
        {
            get; set;
        }
        public DbSet<Player> Players
        {
            get; set;
        }
        public DbSet<Board> Boards
        {
            get; set;
        }
    }
}
