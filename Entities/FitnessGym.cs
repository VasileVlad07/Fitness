using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Fitness.Entities
{
    public partial class FitnessGym : DbContext
    {
        public FitnessGym()
        {
        }

        public FitnessGym(DbContextOptions<FitnessGym> options)
            : base(options)
        {
        }

        public virtual DbSet<InstructorInformation> InstructorInformations { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server = tcp:fitnessappdataserver.database.windows.net, 1433; Initial Catalog = Fitness; Persist Security Info = False; User ID = fitnessappserver; Password = legarebazadedate!7; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;");
                // "Server=localhost;Initial Catalog=Fitness; Persist Security Info=false;  User Id=fitnessuser; Password=test123$"
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InstructorInformation>(entity =>
            {
                entity.HasKey(e => e.UserId);


                entity.ToTable("InstructorInformation");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Hours).HasColumnName("Hours");

                entity.Property(e => e.InstructorId).HasColumnName("InstructorID");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
