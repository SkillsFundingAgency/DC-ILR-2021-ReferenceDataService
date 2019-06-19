using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model
{
    public partial class IlrReferenceDataContext : DbContext
    {
        public IlrReferenceDataContext()
        {
        }

        public IlrReferenceDataContext(DbContextOptions<IlrReferenceDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Lookup> Lookups { get; set; }
        public virtual DbSet<LookupSubCategory> LookupSubCategories { get; set; }
        public virtual DbSet<Rule> Rules { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\;Database=ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Database;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<Lookup>(entity =>
            {
                entity.HasKey(e => new { e.Name, e.Code });

                entity.ToTable("Lookup");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.EffectiveFrom).HasColumnType("date");

                entity.Property(e => e.EffectiveTo).HasColumnType("date");
            });

            modelBuilder.Entity<LookupSubCategory>(entity =>
            {
                entity.HasKey(e => new { e.ParentName, e.Name, e.Code });

                entity.ToTable("LookupSubCategory");

                entity.Property(e => e.ParentName).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(400);

                entity.Property(e => e.EffectiveFrom).HasColumnType("date");

                entity.Property(e => e.EffectiveTo).HasColumnType("date");

                entity.HasOne(d => d.Lookup)
                    .WithMany(p => p.LookupSubCategories)
                    .HasForeignKey(d => new { d.ParentName, d.Name })
                    .HasConstraintName("FK_LookupSubCategory_ToLookup");
            });

            modelBuilder.Entity<Rule>(entity =>
            {
                entity.HasKey(e => e.Rulename)
                    .HasName("PK_dbo_Rules");

                entity.Property(e => e.Rulename)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Desktop)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Message).HasMaxLength(2000);

                entity.Property(e => e.Online)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Severity).HasMaxLength(1);
            });
        }
    }
}
