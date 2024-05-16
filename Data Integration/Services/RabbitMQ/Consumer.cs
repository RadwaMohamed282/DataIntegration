using Data_Integration.Models;
using Data_Integration.Services.ProductDetails;
using DeliveryIntegration.Configrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
        public IServiceProvider _services { get; }
        private readonly IModel _channel;
        private readonly ILogger<Consumer> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public Consumer(ApplicationDbContext dbContext,IOptions<RabbitMQConfig> options, IServiceProvider services, ILogger<Consumer> logger, IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _rabbitMQConfig = options.Value;
            _services = services;
            _logger = logger;
            var factory = new ConnectionFactory() { HostName = _rabbitMQConfig.HostName, UserName = _rabbitMQConfig.UserName, Password = _rabbitMQConfig.Password };
            //   Console.WriteLine(_rabbitMQConfig.HostName);
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
           // _dbContext = dbContext;
            _contextFactory = contextFactory;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _channel.QueueDeclare(queue: _rabbitMQConfig.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                //    _channel.BasicAck(ea.DeliveryTag, false);
                _logger.LogInformation("Received message: {Message}", message);

                //await UpdateProductData(message);

            };

            _channel.BasicConsume(queue: _rabbitMQConfig.QueueName, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }

        //private async Task UpdateProductData(string message)
        //{
        //    try
        //    {
        //        using (var context = _contextFactory.CreateDbContext())
        //        {
        //            var scope = _services.CreateScope();

        //            var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IProductServices>();

        //            Product ProductDetails = JsonConvert.DeserializeObject<Product>(message);

        //            await scopedProcessingService.AddProducts(ProductDetails);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Error saving message to database" + ex);
        //    }
        //}
    }
}

