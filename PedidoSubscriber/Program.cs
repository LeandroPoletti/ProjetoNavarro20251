using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using ProjetoEntidades.Models;

namespace DLQ_Projeto.Services
{
    public class PedidoSubscriber
    {
        private static readonly string _hostname = "localhost";
        private static readonly string _queueName = "fila.pedidos";

        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = _hostname };
            var connection = factory.CreateConnection();
            
            // Inicia um canal de comunicação com o RabbitMQ
            using var channel = connection.CreateModel();
            
            //Vincula o channel com a seguinte queue
            channel.QueueDeclare(_queueName, true, false, false, null);
            
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    var item = JsonSerializer.Deserialize<Pedido>(ea.Body.ToArray());
                    channel.BasicAck(ea.DeliveryTag, false); // Confirma processamento
                    //Fazer request pro projeto api receiver
                }
                catch (Exception e)
                {
                    channel.BasicNack(ea.DeliveryTag, false, false); // Não confirma, vai para DLQ
                }
                
                
            };

            channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
        }
    }
}