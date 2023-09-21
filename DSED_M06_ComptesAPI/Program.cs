using DSED_M06_CasUtilisation;
using DSED_M06_ComptesAPI.Data;
using DSED_M06_ComptesAPI.Producteur;
using DSED_M06_Entites;
using DSED_M06_SQLServerDAL;
using DSED_M06_SQLServerDAL.Depots;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DSED_M06_ComptesAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("CompteConnection") ?? throw new InvalidOperationException("Connection string 'CompteConnection' not found.");
            builder.Services.AddDbContext<CompteContextSQLServer>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<CompteContextSQLServer>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<IProducteur, ProducteurMessages>();
            builder.Services.AddScoped<IDepotComptes, DepotCompte>();
            builder.Services.AddScoped<IDepotTransactions, DepotTransaction>();
            builder.Services.AddScoped<ManipulationComptes>();
            builder.Services.AddScoped<ManipulationTransactions>();
            builder.Services.AddSwaggerDocument();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.Run();
        }
    }
}