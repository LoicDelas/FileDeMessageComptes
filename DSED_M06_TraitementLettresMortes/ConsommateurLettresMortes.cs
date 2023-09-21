using DSED_M06_Entites;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DSED_M06_TraitementLettresMortes
{
    public class ConsommateurLettresMortes : IConsommateur
    {
        private static ManualResetEvent _waitHandle = new ManualResetEvent(false);
        private ConnectionFactory m_factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
        private string m_nomFileMessages = "m06-comptes-lettres-mortes";
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
                        Console.WriteLine("Réception message lettre morte");
                        byte[] body = ea.Body.ToArray();
                        EnregistrerLettreMorte(body);
                        channel.BasicAck(ea.DeliveryTag, false);
                    };

                    channel.BasicConsume(queue: this.m_nomFileMessages, autoAck: false, consumer: consommateur);

                    _waitHandle.WaitOne();
                }
            }
        }

        private void EnregistrerLettreMorte(byte[] body)
        {
            string cheminFichier = Path.Combine("D:\\tmp\\TransactionsEnErreurs", $"{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid()}.bin");
            File.WriteAllBytes(cheminFichier, body);
            Console.WriteLine($"Message lettre morte enregistré dans {cheminFichier}");
        }
    }
}
