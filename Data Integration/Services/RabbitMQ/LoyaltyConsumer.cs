using Data_Integration.Models;
using DeliveryIntegration.Configrations;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Data_Integration.Services.RabbitMQ
{
    public class LoyaltyConsumer : BackgroundService
    {
        private readonly RabbitMQConfig _rabbitMQConfig;
        private readonly ILogger<CouponzConsumer> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IModel _channel;

        public LoyaltyConsumer(IOptions<RabbitMQConfig> options, ILogger<CouponzConsumer> logger, IServiceProvider serviceProvider)
        {
            _rabbitMQConfig = options.Value;
            _logger = logger;
            _serviceProvider = serviceProvider;

            var factory = new ConnectionFactory() {
                HostName = _rabbitMQConfig.HostName, 
                UserName = _rabbitMQConfig.UserName, 
                Password = _rabbitMQConfig.Password 
            };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Consumer running");

            _channel.QueueDeclare(queue: _rabbitMQConfig.LoyaltyQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                await ProcessMessageAsync(message);
            };

            _channel.BasicConsume(queue: _rabbitMQConfig.LoyaltyQueue, autoAck: true, consumer: consumer);
            _logger.LogInformation("QueueName: ", _rabbitMQConfig.LoyaltyQueue);

            await Task.CompletedTask;
        }

        private async Task ProcessMessageAsync(string message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                try
                {
                    var loyalty = JsonConvert.DeserializeObject<List<RewardLoyalty>>(message);
                    if (loyalty == null || !loyalty.Any())
                    {
                        throw new InvalidOperationException("No valid subscriptions found in the input data.");
                    }

                    dbContext.RewardLoyaltys.AddRange(loyalty);

                    await dbContext.SaveChangesAsync();

                    _logger.LogInformation("Message processed successfully: {Message}", message);
                }
                catch (JsonSerializationException ex)
                {
                    _logger.LogError("JSON deserialization error:", ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message: {Message}", message);
                }
            }
        }
    }
}
