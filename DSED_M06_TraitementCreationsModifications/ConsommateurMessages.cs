using DSED_M06_CasUtilisation;
using DSED_M06_Entites;
using DSED_M06_MessageCompte;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace DSED_M06_TraitementCreationsModifications
{
    public class ConsommateurMessages : IConsommateur
    {
        private ManipulationComptes m_manipulationComptes;
        private ManipulationTransactions m_manipulationTransactions;
        private static ManualResetEvent _waitHandle = new ManualResetEvent(false);
        private ConnectionFactory m_factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
        private string m_nomFileMessages = "m06-comptes";
        private IProducteur m_producteurLettresMortes;

        public ConsommateurMessages(ManipulationComptes p_manipulationComptes, ManipulationTransactions p_manipulationTransactions, IProducteur p_producteurLettresMortes)
        {
            this.m_manipulationComptes = p_manipulationComptes ?? throw new ArgumentNullException(nameof(p_manipulationComptes));
            this.m_manipulationTransactions = p_manipulationTransactions ?? throw new ArgumentNullException(nameof(p_manipulationTransactions));
            this.m_producteurLettresMortes = p_producteurLettresMortes ?? throw new ArgumentNullException(nameof(p_producteurLettresMortes));
        }

        public void TraiterMessage()
        {
            using (IConnection connection = this.m_factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: this.m_nomFileMessages, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    EventingBasicConsumer consommateur = new EventingBasicConsumer(channel);
                    consommateur.Received += (model, ea) =>
                    {
                        Console.WriteLine("Réception du message");
                        byte[] body = ea.Body.ToArray();
                        string enveloppeJSON = Encoding.UTF8.GetString(body);
                        try
                        {
                            Enveloppe enveloppe = JsonConvert.DeserializeObject<Enveloppe>(enveloppeJSON);
                            EnregistrerEnveloppe(enveloppe);
                            Console.WriteLine("Message enregistré dans BD");
                            Console.WriteLine();
                            channel.BasicAck(ea.DeliveryTag, false);
                        }
                        catch (JsonSerializationException exception)
                        {
                            Console.WriteLine($"Erreur désérialisation enveloppe :" + exception.Message);
                            this.m_producteurLettresMortes.AjouterMessage(enveloppeJSON);
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine($"Erreur enregistrement enveloppe :", exception.Message);
                            this.m_producteurLettresMortes.AjouterMessage(enveloppeJSON);
                        }
                    };

                    channel.BasicConsume(queue: this.m_nomFileMessages, autoAck: false, consumer: consommateur);

                    _waitHandle.WaitOne();
                }
            }
        }

        private void EnregistrerEnveloppe(Enveloppe p_enveloppe)
        {

            switch (p_enveloppe.Action)
            {
                case TypeAction.Creation:
                    Compte compteCree = ExtraireCompte(p_enveloppe);
                    this.m_manipulationComptes.AjouterCompte(compteCree);
                    break;
                case TypeAction.Modification:
                    Compte compteModifie = ExtraireCompte(p_enveloppe);
                    this.m_manipulationComptes.ModifierCompte(compteModifie);
                    break;
                case TypeAction.CreationTransaction:
                    Transaction transactionCreee = ExtraireTransaction(p_enveloppe);
                    this.m_manipulationTransactions.AjouterTransaction(transactionCreee);
                    break;
            }
        }

        private Transaction ExtraireTransaction(Enveloppe p_enveloppe)
        {
            if (p_enveloppe is null)
            {
                throw new ArgumentNullException(nameof(p_enveloppe));
            }

            MessageTransaction messageTransaction = p_enveloppe.Transaction;

            if (messageTransaction is null)
            {
                throw new ArgumentOutOfRangeException(nameof(p_enveloppe));
            }

            Transaction transaction = new Transaction(
                messageTransaction.Identifiant,
                messageTransaction.Type,
                messageTransaction.Date,
                messageTransaction.Montant,
                messageTransaction.NumeroCompte
                );

            return transaction;
        }

        private Compte ExtraireCompte(Enveloppe p_enveloppeCompte)
        {
            if (p_enveloppeCompte is null)
            {
                throw new ArgumentNullException(nameof(p_enveloppeCompte));
            }

            MessageCompte messageCompte = p_enveloppeCompte.Compte;

            if (messageCompte is null)
            {
                throw new ArgumentOutOfRangeException(nameof(p_enveloppeCompte));
            }

            Compte compte = new Compte(
                messageCompte.Numero,
                messageCompte.Type
                //messageCompte.Transactions
                //    .Select(t => new Transaction(t.Identifiant, t.Type, t.Date, t.Montant))
                //    .ToList()
                );

            return compte;
        }
    }
}
