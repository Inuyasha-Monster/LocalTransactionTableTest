using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Productor.Data.EntityTypeMapConfig
{
    public class OrderDetailMapConfig : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnType("char(36)");
            builder.Property(x => x.OrderNo).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Sku).IsRequired().HasMaxLength(50);
            builder.Property(x => x.SkuName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Quantity).IsRequired().HasColumnName("Quantity");
            builder.Property(x => x.Price).IsRequired().HasColumnName("Price");
            builder.Property(x => x.CreateTime).IsRequired().ValueGeneratedOnAdd();
        }
    }
}
