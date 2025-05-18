using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PedidoPublisherApi;
using ProjetoEntidades.Models;
using RabbitMQ.Client;

namespace PedidoPublisherApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }


    [HttpPost]
    public IActionResult PublicarPedido([FromBody] Pedido pedido)
    {
        string _hostname = "amqps://ywlcmpwn:p5lsey2O43JZhEuJBUE7T-mz79diECJZ@jackal.rmq.cloudamqp.com/ywlcmpwn\n";
        string MainExchange = "direct_logs";
        string DeadLetterExchange = "dlx_direct_logs";
        string MainQueue = "queue.direct_logs";
        string DeadLetterQueue = "queue.direct_logs.dlq";
        var factory = new ConnectionFactory() { Uri = new Uri(_hostname) };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        // Configuração da fila principal com DLQ
        channel.ExchangeDeclare(DeadLetterExchange, ExchangeType.Direct, durable: true);
        channel.QueueDeclare(
            queue: DeadLetterQueue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        channel.QueueBind(DeadLetterQueue, DeadLetterExchange, routingKey: "error");

        // Declare main exchange and queue with DLX argument
        channel.ExchangeDeclare(MainExchange, ExchangeType.Direct, durable: true);
        var args = new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", DeadLetterExchange },
            { "x-dead-letter-routing-key", "error" }
        };
        channel.QueueDeclare(
            queue: MainQueue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: args);
        channel.QueueBind(MainQueue, MainExchange, routingKey: "info");

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(pedido));
        channel.BasicPublish(
            exchange: MainExchange,
            routingKey: "info",
            basicProperties: null,
            body: body);

        return Ok();
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}