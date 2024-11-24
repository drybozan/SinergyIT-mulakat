using CRUDApplication.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CRUDApplication.Data.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasKey(c => c.id);

            modelBuilder.Entity<Order>()
                .HasKey(o => o.id);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.customer)
                .WithMany(c => c.orders)
                .HasForeignKey(o => o.customerId);
        }
    }
}
