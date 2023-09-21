using DSED_M06_Entites;
using Microsoft.Extensions.Configuration;
using System;

namespace DSED_M06_TraitementLettresMortes
{
    public class Program
    {
        private static IConsommateur m_consommateurLettreMorte;
        
        static void Main(string[] args)
        {
            Executer();
        }

        private static void Executer()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
            string repertoireLettreMorte = configuration["DossierLettresMortes"];

            m_consommateurLettreMorte = new ConsommateurLettresMortes();
            GenererDossierSiNonExistant(repertoireLettreMorte);
            m_consommateurLettreMorte.TraiterMessage();
        }

        private static void GenererDossierSiNonExistant(string p_chemin)
        {
            if (!Directory.Exists(p_chemin))
            {
                Directory.CreateDirectory(p_chemin);
            }
        }
    }
}