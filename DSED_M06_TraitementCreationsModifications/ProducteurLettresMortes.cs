using DSED_M06_Entites;
using DSED_M06_MessageCompte;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSED_M06_TraitementCreationsModifications
{
    public class ProducteurLettresMortes : IProducteur
    {
        private static ManualResetEvent _waitHandle = new ManualResetEvent(false);
        private ConnectionFactory m_factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
        private string m_nomFileMessages = "m06-comptes-lettres-mortes";

        public void AjouterMessage(string p_message)
        {
            if (p_message is null)
            {
                throw new ArgumentNullException(nameof(p_message));
            }

            using (IConnection connexion = this.m_factory.CreateConnection())
            {
                using (IModel channel = connexion.CreateModel())
                {
                    channel.QueueDeclare(queue: this.m_nomFileMessages,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    byte[] body = Encoding.UTF8.GetBytes(p_message);
                    channel.BasicPublish(exchange: "", routingKey: this.m_nomFileMessages, body: body);
                }
            }
        }
    }
}
