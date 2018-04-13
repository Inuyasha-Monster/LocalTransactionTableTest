using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Productor.Data.EntityTypeMapConfig
{
    public class OrderHeaderMapConfig : IEntityTypeConfiguration<OrderHeader>
    {
        public void Configure(EntityTypeBuilder<OrderHeader> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnType("char(36)");
            builder.Property(x => x.AppUser).IsRequired().HasMaxLength(20);
            builder.Property(x => x.CreateTime).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Amount).IsRequired().HasColumnName("Amount");
        }
    }
}
