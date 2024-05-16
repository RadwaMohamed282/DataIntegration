using Data_Integration.Models;
using DeliveryIntegration.Configrations;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Data_Integration.Services.RabbitMQ
{
    public class Consumer : BackgroundService
    {
        private readonly RabbitMQConfig _rabbitMQConfig;
        private readonly ILogger<Consumer> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IModel _channel;

        public Consumer(IOptions<RabbitMQConfig> options, ILogger<Consumer> logger, IServiceProvider serviceProvider)
        {
            _rabbitMQConfig = options.Value;
            _logger = logger;
            _serviceProvider = serviceProvider;

            var factory = new ConnectionFactory() { HostName = _rabbitMQConfig.HostName, UserName = _rabbitMQConfig.UserName, Password = _rabbitMQConfig.Password };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Consumer running");

            _channel.QueueDeclare(queue: _rabbitMQConfig.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                await ProcessMessageAsync(message);
            };

            _channel.BasicConsume(queue: _rabbitMQConfig.QueueName, autoAck: true, consumer: consumer);

            await Task.CompletedTask;
        }

        private async Task ProcessMessageAsync(string message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var coupon = JsonConvert.DeserializeObject<SubscribeToOffer>(message);

                dbContext.SubscribeToOffers.Add(coupon);

                try
                {
                    await dbContext.SaveChangesAsync();
                    _logger.LogInformation("Message processed successfully: {Message}", message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message: {Message}", message);
                }
            }
        }
    }
}
