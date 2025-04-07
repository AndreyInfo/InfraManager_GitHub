using System;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;

namespace IM.Core.BLL.Test
{
    internal class TestDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<KBTag>(entity =>
            {
                entity.Property(x => x.Id)
                    .IsRequired()
                    .HasColumnName("ID");
                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("Name");
            });
        }
    }
}