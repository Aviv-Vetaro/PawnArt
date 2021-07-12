using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PawnArt.Logic.BoardArrangment;

namespace PawnArt.DataAccess.EntityTypeConfigurations
{
    public class PieceTypeConfiguration : IEntityTypeConfiguration<Piece>
    {
        public void Configure(EntityTypeBuilder<Piece> builder)
        {
            builder.ToTable("Pieces");

            builder.Property(p => p.Color);
            builder.Property(p => p.Moved);
            builder.Property(p => p.Type);
            builder.Property<Guid>("Id");
            builder.HasKey("Id");
        }
    }
}
