
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using PawnArt.Logic;
using PawnArt.Logic.BoardArrangment;

namespace PawnArt.DataAccess.EntityTypeConfigurations
{
    internal class GameTypeConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            //builder.Property(g => g.WhitePlayer);
            //builder.Property(g => g.BlackPlayer);
            builder.Property(g => g.Recurring);
            builder.ToTable("Games");

            builder.HasKey(g => g.Id);
            builder.Ignore(g => g.Squares);
            builder.HasOne(g => g.WhitePlayer).WithMany().OnDelete(DeleteBehavior.ClientCascade);
            builder.HasOne(g => g.BlackPlayer).WithMany().OnDelete(DeleteBehavior.ClientCascade);

            builder.OwnsOne<ClockState>("state", b =>
            {
                b.Property("whitePlayerTime").HasConversion(new TimeSpanToStringConverter()).IsRequired(true);
                b.Property("blackPlayerTime").HasConversion(new TimeSpanToStringConverter()).IsRequired(true);
                b.Property("lastPlyTime").HasConversion(new DateTimeOffsetToStringConverter()).IsRequired(true);
            });

            builder.HasOne<Board>(g => g.Board).WithOne().HasForeignKey<Board>("GameId");


        }
    }
}
