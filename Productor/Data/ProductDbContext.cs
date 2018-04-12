using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Productor.Data.EntityTypeMapConfig;

namespace Productor.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MqMessageTypeConfig());
            modelBuilder.ApplyConfiguration(new OrderDetailMapConfig());
            modelBuilder.ApplyConfiguration(new OrderHeaderMapConfig());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<MqMessage> MqMessages { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
