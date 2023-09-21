using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSED_M06_Entites
{
    public class Transaction
    {
        public string Identifiant { get; private set; }
        public string Type { get; private set; }
        public DateTime Date { get; private set; }
        public decimal Montant { get; private set; }
        public string NumeroCompte { get; set; }

        private const int ANNEE_MAX = 1950;

        public Transaction(string p_identifiant, string p_type, DateTime p_date, decimal p_montant, string p_numeroCompte)
        {
            if (string.IsNullOrWhiteSpace(p_identifiant))
            {
                throw new ArgumentException($"'{nameof(p_identifiant)}' ne peut pas avoir une valeur null ou être un espace blanc.", nameof(p_identifiant));
            }

            if (string.IsNullOrWhiteSpace(p_type))
            {
                throw new ArgumentException($"'{nameof(p_type)}' ne peut pas avoir une valeur null ou être un espace blanc.", nameof(p_type));
            }

            if (p_date > DateTime.Now || p_date.Year < ANNEE_MAX)
            {
                throw new ArgumentOutOfRangeException($"La date doit être antérieure à la date courrante et postérieure à {ANNEE_MAX}", nameof(p_date));
            }

            this.Identifiant = p_identifiant;
            this.Type = p_type;
            this.Date = p_date;
            this.Montant = p_montant;
            this.NumeroCompte = p_numeroCompte;
        }
    }
}
