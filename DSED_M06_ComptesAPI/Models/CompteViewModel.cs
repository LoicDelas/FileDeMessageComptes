using DSED_M06_Entites;

namespace DSED_M06_ComptesAPI.Models
{
    public class CompteViewModel
    {
        public string Numero { get; set; }
        public string Type { get; set; }
        //public IEnumerable<TransactionViewModel> Transactions { get; set; }

        public CompteViewModel()
        {
            ;
        }
        public CompteViewModel(Compte p_compte)
        {
            this.Numero = p_compte.Numero;
            this.Type = p_compte.Type;
            //this.Transactions = p_compte.Transactions.ToList().Select(t => new TransactionViewModel(t));
        }

        public Compte VersEntite()
        {
            return new Compte(this.Numero, this.Type/*, this.Transactions.ToList().Select(t => t.VersEntite())*/);
        }
    }
}
