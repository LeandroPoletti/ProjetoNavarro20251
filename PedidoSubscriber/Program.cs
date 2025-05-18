using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using ProjetoEntidades.Models;

namespace DLQ_Projeto.Services
{
    public class PedidoSubscriber
    {
        private static readonly string _hostname =
            "amqps://ywlcmpwn:p5lsey2O43JZhEuJBUE7T-mz79diECJZ@jackal.rmq.cloudamqp.com/ywlcmpwn\n";

        private const string MainExchange = "direct_logs";
        private const string MainQueue = "queue.direct_logs";
        private const string DeadLetterQueue = "queue.direct_logs.dlq";
        private const string DeadLetterExchange = "dlx_direct_logs";

        public static void Main()
        {
            var factory = new ConnectionFactory() { Uri = new Uri(_hostname) };
            var connection = factory.CreateConnection();

            // Inicia um canal de comunicação com o RabbitMQ
            using var channel = connection.CreateModel();

            //Vincula o channel com a seguinte queue
            channel.ExchangeDeclare(MainExchange, ExchangeType.Direct, durable: true);
            channel.QueueDeclare(queue: MainQueue, durable: true, exclusive: false, autoDelete: false,
                arguments: new Dictionary<string, object>
                {
                    { "x-dead-letter-exchange", DeadLetterExchange },
                    { "x-dead-letter-routing-key", "error" }
                });
            channel.QueueBind(MainQueue, MainExchange, routingKey: "info");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);

                var item = JsonSerializer.Deserialize<Pedido>(ea.Body.ToArray());

                if (string.IsNullOrEmpty(item.NomeCliente))
                {
                    channel.BasicNack(ea.DeliveryTag, false, false); // Não confirma, vai para DLQ
                }
                else
                {
                    channel.BasicAck(ea.DeliveryTag, false); // Confirma processamento
                }

                //Fazer request pro projeto api receiver
            };

            channel.BasicConsume(queue: MainQueue, autoAck: false, consumer: consumer);
            Console.WriteLine("\n Iniciando processo...");
            Console.ReadLine();
        }
    }
}