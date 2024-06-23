using Data_Integration.Models;
using DeliveryIntegration.Configrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Data_Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponzController : ControllerBase
    {
        private readonly RabbitMQConfig _rabbitMQConfig;
        private readonly ApplicationDbContext _dbContext;
        public CouponzController(IOptions<RabbitMQConfig> options, ApplicationDbContext dbContext)
        {
            _rabbitMQConfig = options.Value;
            _dbContext = dbContext;
        }

        [HttpGet("GetAllCouponz")]
        public Task<IActionResult> GetAllCouponz()
        {
            var result = _dbContext.SubscribeToOffers.ToList();
            return Task.FromResult<IActionResult>(Ok(result));
        }

        [HttpPost("AddNewCoupon")]
        public Task<IActionResult> AddNewCouponz(SubscribeToOfferDto subscribeToOfferDto)
        {
            var subscribeToOffer = new SubscribeToOffer()
            {
                CouponNumber = subscribeToOfferDto.CouponNumber,
                MSISDN = subscribeToOfferDto.MSISDN
            };
            _dbContext.SubscribeToOffers.Add(subscribeToOffer);
            _dbContext.SaveChanges();

            return Task.FromResult<IActionResult>(Ok(subscribeToOffer));
        }
    }
}
