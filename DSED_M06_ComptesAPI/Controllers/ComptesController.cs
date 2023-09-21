using DSED_M06_CasUtilisation;
using DSED_M06_ComptesAPI.Models;
using DSED_M06_Entites;
using DSED_M06_MessageCompte;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DSED_M06_ComptesAPI.Controllers
{
    [Route("api/comptes")]
    [ApiController]
    public class ComptesController : ControllerBase
    {
        private ManipulationComptes m_manipulationComptes;
        private IProducteur m_producteurComptes;

        public ComptesController(ManipulationComptes p_manipulationComptes, IProducteur p_producteurComptes)
        {
            this.m_manipulationComptes = p_manipulationComptes ?? throw new ArgumentNullException(nameof(p_manipulationComptes));
            this.m_producteurComptes = p_producteurComptes ?? throw new ArgumentNullException(nameof(p_producteurComptes));
        }

        // GET: api/comptes
        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult<IEnumerable<CompteViewModel>> Get()
        {
            return Ok(this.m_manipulationComptes.ObtenirComptes().Select(c => new CompteViewModel(c)));
        }

        // GET api/comptes/5
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<CompteViewModel> Get(string id)
        {
            Compte compte = this.m_manipulationComptes.ObtenirCompte(id);

            if (compte is null)
            {
                return NotFound();
            }

            return Ok(new CompteViewModel(compte));
        }

        // POST api/comptes
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult Post([FromBody] CompteViewModel compte)
        {
            if (!this.ModelState.IsValid || !EstValide(compte))
            {
                return BadRequest();
            }

            if (this.m_manipulationComptes.ObtenirCompte(compte.Numero) is not null)
            {
                return BadRequest();
            }

            this.EnvoyerEnveloppeCompte(compte, TypeAction.Creation);

            return CreatedAtAction(nameof(Compte), new { id = compte.Numero }, compte);
        }

        // PUT api/comptes/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult Put(string id, [FromBody] CompteViewModel compte)
        {
            if (!this.ModelState.IsValid || !EstValide(compte) || id != compte.Numero)
            {
                return BadRequest();
            }

            if (this.m_manipulationComptes.ObtenirCompte(compte.Numero) is null)
            {
                return NotFound();
            }

            this.EnvoyerEnveloppeCompte(compte, TypeAction.Modification);

            return NoContent();
        }

        // DELETE api/comptes/5
        [HttpDelete("{id}")]
        [ProducesResponseType(403)]
        public ActionResult Delete(string id)
        {
            return Forbid();
        }

        private static bool EstValide(CompteViewModel p_compte)
        {
            return !string.IsNullOrWhiteSpace(p_compte.Numero)
                && !string.IsNullOrWhiteSpace(p_compte.Type);
        }

        private void EnvoyerEnveloppeCompte(CompteViewModel p_compte, TypeAction p_action)
        {
            if (p_compte is null)
            {
                throw new ArgumentNullException(nameof(p_compte));
            }

            //List<MessageTransaction> messageTransactions = p_compte.Transactions is null ?
            //    new List<MessageTransaction>() :
            //    p_compte.Transactions.Select(t => new MessageTransaction(t.Identifiant, t.Type, t.Date, t.Montant)).ToList();

            MessageCompte messageCompte = new MessageCompte(p_compte.Numero, p_compte.Type/*, messageTransactions*/);

            Enveloppe enveloppeCompte = new Enveloppe(p_action, messageCompte);
            
            string enveloppeCompteJSON = JsonConvert.SerializeObject(enveloppeCompte);

            this.m_producteurComptes.AjouterMessage(enveloppeCompteJSON);
        }
    }
}
