using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PawnArt.Logic;

namespace PawnArt.DataAccess.EntityTypeConfigurations
{
    internal class PlayerTypeConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.ToTable("Players");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Rank);
            builder.Property(p => p.UserName).HasMaxLength(256);
        }
    }
}
