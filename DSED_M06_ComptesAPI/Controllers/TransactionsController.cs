using DSED_M06_CasUtilisation;
using DSED_M06_ComptesAPI.Models;
using DSED_M06_Entites;
using DSED_M06_MessageCompte;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DSED_M06_ComptesAPI.Controllers
{
    [Route("api/comptes/{compteId}/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private ManipulationTransactions m_manipulationTransactions;
        private IProducteur m_producteurMessage;

        public TransactionsController(ManipulationTransactions p_manipulationTransactions, IProducteur p_producteur)
        {
            this.m_manipulationTransactions = p_manipulationTransactions ?? throw new ArgumentNullException(nameof(p_manipulationTransactions));
            this.m_producteurMessage = p_producteur ?? throw new ArgumentNullException(nameof(p_producteur));
        }

        // GET: api/comptes/1/transactions
        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult<IEnumerable<TransactionViewModel>> Get(string compteId)
        {
            return Ok(this.m_manipulationTransactions.ObtenirTransactions(compteId));
        }

        // GET api/comptes/1/transactions/5
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<TransactionViewModel> Get(string compteId, string id)
        {
            Transaction transaction = this.m_manipulationTransactions.ObtenirTransaction(id);

            if (transaction is null)
            {
                return NotFound();
            }

            if (transaction.NumeroCompte != compteId)
            {
                return BadRequest();
            }

            return Ok(new TransactionViewModel(transaction));
        }

        // POST api/comptes/1/transactions
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult Post(string numeroCompte, [FromBody] TransactionViewModel transaction)
        {
            if (!this.ModelState.IsValid || !EstValide(transaction))
            {
                return BadRequest();
            }

            if (this.m_manipulationTransactions.ObtenirTransaction(transaction.Identifiant) is not null)
            {
                return BadRequest();
            }

            this.EnvoyerEnveloppeTransaction(transaction);

            return CreatedAtAction(nameof(Compte), new { id = transaction.Identifiant }, transaction);
        }

        // PUT api/comptes/1/transactions/5
        [HttpPut("{id}")]
        [ProducesResponseType(403)]
        public ActionResult Put(string id, [FromBody] string value)
        {
            return Forbid();
        }

        // DELETE api/comptes/1/transactions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return Forbid();
        }

        private void EnvoyerEnveloppeTransaction(TransactionViewModel p_transaction)
        {
            if (p_transaction is null)
            {
                throw new ArgumentNullException(nameof(p_transaction));
            }

            //List<MessageTransaction> messageTransactions = p_transaction.Transactions is null ?
            //    new List<MessageTransaction>() :
            //    p_transaction.Transactions.Select(t => new MessageTransaction(t.Identifiant, t.Type, t.Date, t.Montant)).ToList();

            MessageTransaction messageTransaction = new MessageTransaction(p_transaction.Identifiant, p_transaction.Type, p_transaction.Date, p_transaction.Montant, p_transaction.NumeroCompte);

            Enveloppe enveloppeTransaction = new Enveloppe(TypeAction.CreationTransaction, messageTransaction);

            string enveloppeJSON = JsonConvert.SerializeObject(enveloppeTransaction);

            this.m_producteurMessage.AjouterMessage(enveloppeJSON);
        }

        private bool EstValide(TransactionViewModel p_transaction)
        {
            return p_transaction.Date <= DateTime.Now
                && p_transaction.Date.Year > 1950
                && (p_transaction.Type == "crédit" || p_transaction.Type == "débit");
        }
    }
}
