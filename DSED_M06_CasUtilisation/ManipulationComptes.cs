using DSED_M06_Entites;

namespace DSED_M06_CasUtilisation
{
    public class ManipulationComptes
    {
        private IDepotComptes m_depotCompte;

        public ManipulationComptes(IDepotComptes depotCompte)
        {
            m_depotCompte = depotCompte ?? throw new ArgumentNullException(nameof(depotCompte));
        }

        public IEnumerable<Compte> ObtenirComptes()
        {
            return this.m_depotCompte.ObtenirComptes();
        }

        public Compte ObtenirCompte(string p_numero)
        {
            if (string.IsNullOrWhiteSpace(p_numero))
            {
                throw new ArgumentException($"'{nameof(p_numero)}' ne peut pas avoir une valeur null ou être un espace blanc.", nameof(p_numero));
            }

            return this.m_depotCompte.ObtenirCompte(p_numero);
        }

        public void AjouterCompte(Compte p_compte)
        {
            if (p_compte is null)
            {
                throw new ArgumentNullException(nameof(p_compte));
            }

            if (!EstValide(p_compte))
            {
                throw new ValidationUnitaireException(nameof(p_compte));
            }
            this.m_depotCompte.AjouterCompte(p_compte);
        }

        public void ModifierCompte(Compte p_compte)
        {
            if (p_compte is null)
            {
                throw new ArgumentNullException(nameof(p_compte));
            }

            if (!EstValide(p_compte))
            {
                throw new ValidationUnitaireException(nameof(p_compte));
            }

            Compte compteOriginal = this.m_depotCompte.ObtenirCompte(p_compte.Numero);

            this.m_depotCompte.ModifierCompte(p_compte);
        }

        private static bool EstValide(Compte p_compte)
        {
            return !string.IsNullOrWhiteSpace(p_compte.Numero)
                && !string.IsNullOrWhiteSpace(p_compte.Type);
        }
    }
}