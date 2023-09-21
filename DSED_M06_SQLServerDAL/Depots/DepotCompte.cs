using DSED_M06_Entites;
using DSED_M06_SQLServerDAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace DSED_M06_SQLServerDAL.Depots
{
    public class DepotCompte : IDepotComptes
    {
        private CompteContextSQLServer m_dbContext;

        public DepotCompte(CompteContextSQLServer p_context)
        {
            m_dbContext = p_context ?? throw new ArgumentNullException(nameof(p_context));
        }
        public void AjouterCompte(Compte p_compte)
        {
            if (!EstValide(p_compte))
            {
                throw new ValidationUnitaireException(nameof(p_compte));
            }

            if (m_dbContext.Compte.Any(c => c.NumeroCompte == p_compte.Numero))
            {
                throw new InvalidOperationException($"Un compte avec le numéro {p_compte.Numero} existe déjà");
            }

            m_dbContext.Add(new CompteSQLDTO(p_compte));
            m_dbContext.SaveChanges();
            m_dbContext.ChangeTracker.Clear();
        }

        public void ModifierCompte(Compte p_compte)
        {
            if (!EstValide(p_compte))
            {
                throw new ValidationUnitaireException(nameof(p_compte));
            }

            if (!m_dbContext.Compte.Any(c => c.NumeroCompte == p_compte.Numero))
            {
                throw new InvalidOperationException($"Un compte avec le numéro {p_compte.Numero} existe déjà");
            }

            CompteSQLDTO compteDTO = new CompteSQLDTO(p_compte);

            m_dbContext.Update(compteDTO);
            m_dbContext.SaveChanges();
            m_dbContext.ChangeTracker.Clear();
        }

        public Compte? ObtenirCompte(string p_numero)
        {
            if (string.IsNullOrWhiteSpace(p_numero))
            {
                throw new ArgumentException($"'{nameof(p_numero)}' ne peut pas avoir une valeur null ou être un espace blanc.", nameof(p_numero));
            }

            //CompteSQLDTO compte = this.m_dbContext.Compte.SingleOrDefault(c => c.NumeroCompte == p_numero);

            CompteSQLDTO? compte = m_dbContext.Compte.Where(c => c.NumeroCompte == p_numero).Include(t => t.Transactions).SingleOrDefault();

            return compte?.VersEntite();
        }

        public IEnumerable<Compte> ObtenirComptes()
        {
            return m_dbContext.Compte.Include(t => t.Transactions).ToList().Select(c => c.VersEntite());
        }

        private static bool EstValide(Compte p_compte)
        {
            return !string.IsNullOrWhiteSpace(p_compte.Numero)
                && !string.IsNullOrWhiteSpace(p_compte.Type);
        }
    }
}