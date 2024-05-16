using Data_Integration;
using Data_Integration.Models;
using Data_Integration.Services.RabbitMQ;
using DeliveryIntegration.Configrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ProducerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly RabbitMQConfig _rabbitMQConfig;
        private readonly ApplicationDbContext _dbContext;

        public ProductController(IOptions<RabbitMQConfig> options, ApplicationDbContext dbContext)
        {
            _rabbitMQConfig = options.Value;
            _dbContext = dbContext;
        }

        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            var result =  _dbContext.Product.ToList();
            return Ok(result);
        }
    }
}
