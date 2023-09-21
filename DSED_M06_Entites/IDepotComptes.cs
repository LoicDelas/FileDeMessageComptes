using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSED_M06_Entites
{
    public interface IDepotComptes
    {
        IEnumerable<Compte> ObtenirComptes();
        Compte ObtenirCompte(string p_numero);
        void AjouterCompte(Compte p_compte);
        void ModifierCompte(Compte p_compte);
    }
}
