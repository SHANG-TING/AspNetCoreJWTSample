using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OneTableRestAPI.Models
{
    public partial class AnonChatContext : DbContext
    {
        public virtual DbSet<UserInfo> UserInfo { get; set; }

        public AnonChatContext(DbContextOptions<AnonChatContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.AboutMe).HasColumnType("ntext");

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasColumnType("nchar(10)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("nchar(32)");
            });
        }
    }
}
