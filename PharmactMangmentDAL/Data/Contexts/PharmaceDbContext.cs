using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PharmactMangmentDAL.Models;

namespace PharmactMangmentDAL.Data.Contexts
{
    public class PharmaceDbContext : IdentityDbContext<AppUser>
    {

        public PharmaceDbContext(DbContextOptions<PharmaceDbContext> options) : base(options)
        {
        }

        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<Med_Phar> Med_Phars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Pharmacy>()
               .HasMany(m => m.Med_Phars)
               .WithOne(p => p.pharmacy)
               .HasForeignKey(m => m.PharmacyId)
               .OnDelete(DeleteBehavior.Cascade); // عند حذف الصيدلية، تُحذف الأدوية المرتبطة بها

            modelBuilder.Entity<Med_Phar>().HasKey(mp => new { mp.MedicationId, mp.PharmacyId });

            modelBuilder.Entity<Pharmacy>()
                .HasOne<AppUser>()
                .WithOne()
                .HasForeignKey<Pharmacy>(p => p.userId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Medication>()
                .Property(x => x.ImageName).IsRequired(false);

        }

    }
}
