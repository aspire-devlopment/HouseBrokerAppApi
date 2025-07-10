using HouseBrokerApp.Application.Interfaces;
using HouseBrokerApp.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseBrokerApp.API.Controllers
{
    [Authorize(Roles = "Broker")]
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyListingService _propertyService;

        public PropertyController(IPropertyListingService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpPost]
        [RequestSizeLimit(10_000_000)] // 10 MB limit
        public async Task<IActionResult> Create([FromForm] CreatePropertyListingRequest request)
        {

            var brokerIdClaim = User.FindFirst("BrokerId")?.Value;

            if (string.IsNullOrEmpty(brokerIdClaim) || !int.TryParse(brokerIdClaim, out var brokerId))
                return Unauthorized(new { Message = "BrokerId not found in token." });

            var id = await _propertyService.CreateAsync(request, brokerId);
            return Ok(new { Id = id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdatePropertyListingRequest request)
        {
            var brokerIdClaim = User.FindFirst("BrokerId")?.Value;

            if (string.IsNullOrEmpty(brokerIdClaim) || !int.TryParse(brokerIdClaim, out var brokerId))
                return Unauthorized(new { Message = "BrokerId not found in token." });

            request.Id = id;
            var updated = await _propertyService.UpdateAsync(request, brokerId);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var brokerIdClaim = User.FindFirst("BrokerId")?.Value;

            if (string.IsNullOrEmpty(brokerIdClaim) || !int.TryParse(brokerIdClaim, out var brokerId))
                return Unauthorized(new { Message = "BrokerId not found in token." });

            var deleted = await _propertyService.DeleteAsync(id, brokerId);
            return deleted ? NoContent() : NotFound();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _propertyService.GetByIdAsync(id, User);
            return result is not null ? Ok(result) : NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var brokerIdClaim = User.FindFirst("BrokerId")?.Value;

            if (string.IsNullOrEmpty(brokerIdClaim) || !int.TryParse(brokerIdClaim, out var brokerId))
                return Unauthorized(new { Message = "BrokerId not found in token." });

            var result = await _propertyService.GetAllByBrokerUserName(brokerId);
            
            return result is not null ? Ok(result) : NotFound();
        }

       
    }

}
