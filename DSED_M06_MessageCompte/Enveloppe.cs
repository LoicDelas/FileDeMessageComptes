using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSED_M06_MessageCompte
{
    public class Enveloppe
    {
        public TypeAction Action { get; set; }
        public string ActionId { get; set; }
        public MessageCompte? Compte { get; set; }
        public MessageTransaction? Transaction { get; set; }

        public Enveloppe()
        {
            ;
        }

        public Enveloppe(TypeAction p_action, MessageCompte p_compte)
        {
            this.Action = p_action;
            this.ActionId = Guid.NewGuid().ToString();
            this.Compte = p_compte;
        }

        public Enveloppe(TypeAction p_action, MessageTransaction p_messageTransaction)
        {
            this.Action = p_action;
            this.ActionId = Guid.NewGuid().ToString();
            this.Transaction = p_messageTransaction;
        }
    }
}
