using DSED_M06_Entites;
using DSED_M06_SQLServerDAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSED_M06_SQLServerDAL.Depots
{
    public class DepotTransaction : IDepotTransactions
    {
        private CompteContextSQLServer m_dbContext;

        public DepotTransaction(CompteContextSQLServer p_context)
        {
            m_dbContext = p_context ?? throw new ArgumentNullException(nameof(p_context));
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

            if (m_dbContext.Transaction.Any(t => t.TransactionId == p_transaction.Identifiant))
            {
                throw new InvalidOperationException($"Une transaction avec l'identifiant {p_transaction.Identifiant} existe déjà");
            }

            m_dbContext.Add(new TransactionSQLDTO(p_transaction));
            m_dbContext.SaveChanges();
            m_dbContext.ChangeTracker.Clear();
        }

        public Transaction? ObtenirTransaction(string p_identifiant)
        {
            if (string.IsNullOrWhiteSpace(p_identifiant))
            {
                throw new ArgumentException($"'{nameof(p_identifiant)}' ne peut pas avoir une valeur null ou être un espace blanc.", nameof(p_identifiant));
            }

            return m_dbContext.Transaction.Where(t => t.TransactionId == p_identifiant).Select(t => t.VersEntite()).SingleOrDefault();
        }

        public IEnumerable<Transaction> ObtenirTransactions(string p_numeroCompte)
        {
            return m_dbContext.Transaction.Where(t => t.NumeroCompte == p_numeroCompte).Select(t => t.VersEntite());
        }

        public IEnumerable<Transaction> ObtenirTransactions()
        {
            throw new NotImplementedException();
        }

        private bool EstValide(Transaction p_transaction)
        {
            return p_transaction.Date <= DateTime.Now
                && p_transaction.Date.Year > 1950
                && (p_transaction.Type == "crédit" || p_transaction.Type == "débit");
        }
    }
}
