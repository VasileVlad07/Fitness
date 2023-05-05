using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Fitness.Entities
{
    public partial class ClientEntity : DbContext
    {
        public ClientEntity()
        {
        }

        public ClientEntity(DbContextOptions<ClientEntity> options)
            : base(options)
        {
        }

        public virtual DbSet<ClientInformation> ClientInformations { get; set; } 
        public virtual DbSet<InstructorInformation> InstructorInformations { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Initial Catalog=Fitness; Persist Security Info=false;  User Id=fitnessuser; Password=test123$");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientInformation>(entity =>
            {
                entity.HasKey(e => e.ClientId);

                entity.ToTable("ClientInformation");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Hours).HasColumnName("Hours");

                entity.Property(e => e.Categories).HasColumnName("Category");

                entity.Property(e => e.InstructorID).HasColumnName("InstructorID");
            });

            modelBuilder.Entity<InstructorInformation>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("InstructorInformation");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.InstructorId).HasColumnName("InstructorID");

                entity.Property(e => e.LastName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
