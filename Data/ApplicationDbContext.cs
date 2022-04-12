using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyShop.Models;

namespace MyShop.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Actor> Actors { get; set; }
        public DbSet<DvDimage> DvDimages { get; set; }
        public DbSet<Dvdcategory> Dvdcategories { get; set; }
        public DbSet<Dvdcopy> Dvdcopies { get; set; }
        public DbSet<Dvdtitle> Dvdtitles { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanType> LoanTypes { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MembershipCategory> MembershipCategories { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Studio> Studios { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DvDimage>(entity =>
            {
                entity.HasOne(d => d.DvDnumberNavigation)
                    .WithMany(p => p.DvDimages)
                    .HasForeignKey(d => d.DvDnumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Image_DVDTitle_FK");
            });

            modelBuilder.Entity<Dvdcategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("DVDCategory_PK");
            });

            modelBuilder.Entity<Dvdcopy>(entity =>
            {
                entity.HasKey(e => e.CopyId)
                    .HasName("DVDCopy_PK");

                entity.HasOne(d => d.Dvd)
                    .WithMany(p => p.Dvdcopies)
                    .HasForeignKey(d => d.DvdId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Copy_DVDTitle_Fk");
            });

            modelBuilder.Entity<Dvdtitle>(entity =>
            {
                entity.HasKey(e => e.DvdId)
                    .HasName("DVDTitle_PK");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Dvdtitles)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("DVDCategory_FK");

                entity.HasOne(d => d.Produce)
                    .WithMany(p => p.Dvdtitles)
                    .HasForeignKey(d => d.ProduceId)
                    .HasConstraintName("Producer_FK");

                entity.HasOne(d => d.Studio)
                    .WithMany(p => p.Dvdtitles)
                    .HasForeignKey(d => d.StudioId)
                    .HasConstraintName("Studio_FK");

                entity.HasMany(d => d.Actors)
                    .WithMany(p => p.Dvds)
                    .UsingEntity<Dictionary<string, object>>(
                        "CastMember",
                        l => l.HasOne<Actor>().WithMany().HasForeignKey("ActorId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("Actor_FK"),
                        r => r.HasOne<Dvdtitle>().WithMany().HasForeignKey("DvdId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("DVDTitle_FK"),
                        j =>
                        {
                            j.HasKey("DvdId", "ActorId").HasName("CastMember_PK");

                            j.ToTable("CastMember");
                        });
            });

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.HasOne(d => d.Copy)
                    .WithMany(p => p.Loans)
                    .HasForeignKey(d => d.CopyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DVDCopy_FK");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Loans)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Member_FK");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Loans)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LoanType_FK");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("MembershipCategory_FK");
            });

            modelBuilder.Entity<MembershipCategory>(entity =>
            {
                entity.HasKey(e => e.MemberCategoryId)
                    .HasName("MembershipCategory_PK");
            });

            modelBuilder.Entity<Studio>(entity =>
            {
                entity.Property(e => e.StudioId).ValueGeneratedNever();
            });

        }
    }
}