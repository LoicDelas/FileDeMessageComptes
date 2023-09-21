using DSED_M06_Entites;

namespace DSED_M06_ComptesAPI.Models
{
    public class TransactionViewModel
    {
        public string Identifiant { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public decimal Montant { get; set; }
        public string NumeroCompte { get; set; }

        public TransactionViewModel()
        {
            ;
        }
        public TransactionViewModel(Transaction p_transaction)
        {
            this.Identifiant = p_transaction.Identifiant;
            this.Type = p_transaction.Type;
            this.Date = p_transaction.Date;
            this.Montant = p_transaction.Montant;
            this.NumeroCompte = p_transaction.NumeroCompte;
        }
        public Transaction VersEntite()
        {
            return new Transaction(this.Identifiant, this.Type, this.Date, this.Montant, this.NumeroCompte);
        }
    }
}
