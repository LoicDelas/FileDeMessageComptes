namespace DSED_M06_MessageCompte
{
    public class MessageTransaction
    {
        public string Identifiant { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public decimal Montant { get; set; }
        public string NumeroCompte { get; set; }

        public MessageTransaction()
        {
            ;
        }

        public MessageTransaction(string p_numero, string p_type, DateTime p_date, decimal p_montant, string p_numeroCompte)
        {
            if (string.IsNullOrWhiteSpace(p_numero))
            {
                throw new ArgumentException($"'{nameof(p_numero)}' ne peut pas avoir une valeur null ou être un espace blanc.", nameof(p_numero));
            }

            if (string.IsNullOrWhiteSpace(p_type))
            {
                throw new ArgumentException($"'{nameof(p_type)}' ne peut pas avoir une valeur null ou être un espace blanc.", nameof(p_type));
            }

            if (string.IsNullOrWhiteSpace(p_numeroCompte))
            {
                throw new ArgumentException($"'{nameof(p_numeroCompte)}' ne peut pas avoir une valeur null ou être un espace blanc.", nameof(p_numeroCompte));
            }

            this.Identifiant = p_numero;
            this.Type = p_type;
            this.Date = p_date;
            this.Montant = p_montant;
            this.NumeroCompte = p_numeroCompte;
        }
    }
}