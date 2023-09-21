using DSED_M06_CasUtilisation;
using DSED_M06_Entites;
using DSED_M06_SQLServerDAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using DSED_M06_SQLServerDAL.Depots;

namespace DSED_M06_TraitementCreationsModifications
{
    public class Program
    {
        private static IConsommateur m_consommateurMessages;
        static void Main(string[] args)
        {

            Executer();
        }

        private static void Executer()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)!.FullName)
                .AddJsonFile("appsettings.json").Build();

            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<CompteContextSQLServer>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CompteConnection"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
#if DEBUG
                .LogTo(message => Debug.WriteLine(message), LogLevel.Information)
                .EnableSensitiveDataLogging()
#endif
            );
            services.AddScoped<IDepotComptes, DepotCompte>()
                    .AddScoped<IDepotTransactions, DepotTransaction>()
                    .AddScoped<ManipulationComptes>()
                    .AddScoped<ManipulationTransactions>()
                    .AddSingleton<IProducteur, ProducteurLettresMortes>()
                    .AddSingleton<ConsommateurMessages>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();


            m_consommateurMessages = serviceProvider.GetService<ConsommateurMessages>();

            m_consommateurMessages.TraiterMessage();
        }
    }
}