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
    public class TransactionSQLDTO
    {
        [Key]
        public string TransactionId { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public decimal Montant { get; set; }
        [ForeignKey("NumeroCompte")]
        public string NumeroCompte { get; set; }

        public TransactionSQLDTO()
        {
            ;
        }

        public TransactionSQLDTO(Transaction p_entite)
        {
            TransactionId = p_entite.Identifiant;
            Type = p_entite.Type;
            Date = p_entite.Date;
            Montant = p_entite.Montant;
            NumeroCompte = p_entite.NumeroCompte;
        }

        public Transaction VersEntite()
        {
            return new Transaction(TransactionId, Type, Date, Montant, NumeroCompte);
        }
    }
}
