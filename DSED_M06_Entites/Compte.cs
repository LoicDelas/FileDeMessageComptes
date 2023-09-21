namespace DSED_M06_Entites
{
    public class Compte
    {
        public string Numero { get; private set; }
        public string Type { get; private set; } = "courant";
        public IEnumerable<Transaction> Transactions { get; private set; }

        public Compte(string p_numero, string p_type, IEnumerable<Transaction>? p_transactions = null)
        {
            if (string.IsNullOrWhiteSpace(p_numero))
            {
                throw new ArgumentException($"'{nameof(p_numero)}' ne peut pas avoir une valeur null ou être un espace blanc.", nameof(p_numero));
            }

            if (string.IsNullOrWhiteSpace(p_type))
            {
                throw new ArgumentException($"'{nameof(p_type)}' ne peut pas avoir une valeur null ou être un espace blanc.", nameof(p_type));
            }

            this.Numero = p_numero;
            this.Type = p_type;
            this.Transactions = p_transactions ?? new List<Transaction>();
        }
    }
}