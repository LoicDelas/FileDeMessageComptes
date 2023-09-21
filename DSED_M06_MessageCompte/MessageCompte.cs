namespace DSED_M06_MessageCompte
{
    public class MessageCompte
    {
        public string Numero { get; set; }
        public string Type { get; set; }
        public IEnumerable<MessageTransaction> Transactions { get; set; }

        public MessageCompte(string p_numero, string p_type, IEnumerable<MessageTransaction> p_transactions = null)
        {
            this.Numero = p_numero;
            this.Type = p_type;
            //this.Transactions = p_transactions ?? new List<MessageTransaction>();
        }

        public MessageCompte()
        {
            ;
        }
    }
}