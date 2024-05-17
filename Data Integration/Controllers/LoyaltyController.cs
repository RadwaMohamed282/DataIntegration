using DeliveryIntegration.Configrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Data_Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoyaltyController : ControllerBase
    {
        private readonly RabbitMQConfig _rabbitMQConfig;
        private readonly ApplicationDbContext _dbContext;
        public LoyaltyController(IOptions<RabbitMQConfig> options, ApplicationDbContext dbContext)
        {
            _rabbitMQConfig = options.Value;
            _dbContext = dbContext;
        }

        [HttpGet("GetRewardLoyalty")]
        public Task<IActionResult> GetRewardLoyalty()
        {
            var result = _dbContext.RewardLoyaltys.ToList();
            return Task.FromResult<IActionResult>(Ok(result));
        }
    }
}
