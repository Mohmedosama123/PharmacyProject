using Microsoft.AspNetCore.Identity;
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

        public void DataSeed()
        {
            SeedUsers();
            SeedRoles();
            SaveChanges();
        }

        private void SeedRoles()
        {
            var adminRole = Roles.Find(Consts.Constants.AdminRoleId);
            var pharmacyRole = Roles.Find(Consts.Constants.PharmacyRoleId);

            if (adminRole == null)
            {
                Roles.Add(new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                    Id = Consts.Constants.AdminRoleId,
                });
            }

            if (pharmacyRole == null)
            {
                Roles.Add(new IdentityRole
                {
                    Name = "Pharmacy",
                    NormalizedName = "Pharmacy".ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                    Id = Consts.Constants.PharmacyRoleId,
                });
            }
        }

        private void SeedUsers()
        {
            var userId = Guid.NewGuid().ToString("D");
            var user = Users.FirstOrDefault(u => u.Email == "admin@pharmacy.eg");

            if (user != null)
                return;

            user = new AppUser
            {
                Id = userId,
                NameOfPharmacy = "No",
                Email = "admin@pharmacy.eg",
                UserName = "admin@pharmacy.eg",
                NormalizedUserName = "admin@pharmacy.eg".ToUpper(),
                NormalizedEmail = "admin@pharmacy.eg".ToUpper(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PhoneNumber = "01000000000",
                TwoFactorEnabled = false,
                PasswordHash = null,
                Area = "Area 1",
                District = "District 1",
                Governorate = "governorate",
                FullAddress = "Address 1",
                License_Number = "123",
                OperatingHours = "24 hours",
                OwnerName = "owner 1",
                SecurityStamp = Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = Guid.NewGuid().ToString("D"),
            };
            var hasher = new PasswordHasher<IdentityUser>();
            user.PasswordHash = hasher.HashPassword(user, "M@hmoud1903");

            Users.Add(user);


            this.UserRoles.Add(new IdentityUserRole<string>
            {
                RoleId = Consts.Constants.AdminRoleId,
                UserId = user.Id
            });
        }
    }
}
