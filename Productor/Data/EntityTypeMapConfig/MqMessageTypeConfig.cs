using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Productor.Data.EntityTypeMapConfig
{
    public class MqMessageTypeConfig : IEntityTypeConfiguration<MqMessage>
    {
        public void Configure(EntityTypeBuilder<MqMessage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnType("char(36)");
            builder.Property(x => x.IsPublished).HasDefaultValue(false);
            builder.Property(x => x.MessageAssemblyName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.MessageClassFullName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Body).IsRequired().HasMaxLength(4000);
            builder.Property(x => x.CreateTime).IsRequired().ValueGeneratedOnAdd();
        }
    }
}
