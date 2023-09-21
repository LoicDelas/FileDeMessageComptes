using DSED_M06_Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSED_M06_CasUtilisation
{
    public class ManipulationTransactions
    {
        private IDepotTransactions m_depotTransaction;
        private const int ANNEE_MAX = 1950;

        public ManipulationTransactions(IDepotTransactions p_depotTransaction)
        {
            this.m_depotTransaction = p_depotTransaction ?? throw new ArgumentNullException(nameof(p_depotTransaction));
        }

        public IEnumerable<Transaction> ObtenirTransactions(string p_numeroCompte)
        {
            if (p_numeroCompte is null)
            {
                throw new ArgumentNullException(nameof(p_numeroCompte));
            }

            return this.m_depotTransaction.ObtenirTransactions(p_numeroCompte);
        }

        public void AjouterTransaction(Transaction p_transaction)
        {
            if (p_transaction is null)
            {
                throw new ArgumentNullException(nameof(p_transaction));
            }

            if (!EstValide(p_transaction))
            {
                throw new ValidationUnitaireException(nameof(p_transaction));
            }

            this.m_depotTransaction.AjouterTransaction(p_transaction);
        }

        public Transaction ObtenirTransaction(string p_identifiant)
        {
            if (string.IsNullOrWhiteSpace(p_identifiant))
            {
                throw new ArgumentException($"'{nameof(p_identifiant)}' ne peut pas avoir une valeur null ou être un espace blanc.", nameof(p_identifiant));
            }

            return this.m_depotTransaction.ObtenirTransaction(p_identifiant);
        }

        private bool EstValide(Transaction p_transaction)
        {
            return p_transaction.Date <= DateTime.Now
                && p_transaction.Date.Year > ANNEE_MAX
                && (p_transaction.Type == "crédit" || p_transaction.Type == "débit");
        }
    }
}
