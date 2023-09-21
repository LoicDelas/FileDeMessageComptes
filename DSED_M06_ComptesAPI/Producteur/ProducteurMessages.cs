using DSED_M06_CasUtilisation;
using DSED_M06_Entites;
using RabbitMQ.Client;
using System.Text;

namespace DSED_M06_ComptesAPI.Producteur
{
    public class ProducteurMessages : IProducteur
    {
        private ConnectionFactory m_factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
        private string m_nomFileMessages = "m06-comptes";

        public void AjouterMessage(string p_message)
        {
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
