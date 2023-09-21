using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSED_M06_Entites
{
    public interface IDepotTransactions
    {
        IEnumerable<Transaction> ObtenirTransactions(string p_numeroCompte);
        IEnumerable<Transaction> ObtenirTransactions();
        Transaction ObtenirTransaction(string p_identifiant);
        void AjouterTransaction(Transaction p_transaction);
    }
}
