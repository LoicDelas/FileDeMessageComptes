using DSED_M06_Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSED_M06_SQLServerDAL.DTO
{
    public class CompteSQLDTO
    {
        [Key]
        public string NumeroCompte { get; set; }
        public string Type { get; set; }
        [ForeignKey("NumeroCompte")]
        public IEnumerable<TransactionSQLDTO> Transactions { get; set; }

        public CompteSQLDTO()
        {
            ;
        }

        public CompteSQLDTO(Compte p_entite)
        {
            NumeroCompte = p_entite.Numero;
            Type = p_entite.Type;
            Transactions = p_entite.Transactions is null ?
                new List<TransactionSQLDTO>() :
                p_entite.Transactions.Select(t => new TransactionSQLDTO(t)).ToList();
        }

        internal Compte VersEntite()
        {
            //List<Transaction> transactions = this.Transactions is null ?
            //    new List<Transaction>() :
            //    this.Transactions.Select((t => t.VersEntite())).ToList();

            return new Compte(NumeroCompte, Type, Transactions?.Select(t => t.VersEntite()).ToList());
        }
    }
}
