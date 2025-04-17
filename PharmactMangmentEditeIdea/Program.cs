using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PharmactMangmentBLL.Interfaces;
using PharmactMangmentBLL.Repositories;
using PharmactMangmentDAL.Data.Contexts;
using PharmactMangmentDAL.Models;

namespace PharmactMangmentEditeIdea
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IPharmaceRepository, PharmaceRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IGenaricRepository<Base>, GenaricRepository<Base>>();
            builder.Services.AddScoped<IMedicationRepository, MedicationRepository>();

            builder.Services.AddDbContext<PharmaceDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<PharmaceDbContext>() /* Register DI for Identity*/
                .AddDefaultTokenProviders();
            // too allow to use the default token providers for email confirmation and password reset


            // Register the Identity Middleware
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/SignUp";
            });

            //builder.Services.AddControllers()ِِِ.AddNewtonsoftJson(); // دعم JSON

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<PharmaceDbContext>();
                dbContext.Database.Migrate();
                dbContext.DataSeed();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=SignIn}/{id?}");

            app.Run();
        }
    }
}
