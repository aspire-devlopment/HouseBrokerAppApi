using HouseBrokerApp.Application.Interfaces;
using HouseBrokerApp.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HouseBrokerApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListController : ControllerBase
    {
        private readonly IPropertyListingService _propertyService;

        public ListController(IPropertyListingService propertyService)
        {
            _propertyService = propertyService;
        }

    
        [Authorize(Roles = "Seeker")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _propertyService.GetByIdAsync(id, User);
            return result is not null ? Ok(result) : NotFound();
        }
 

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll() => 
            Ok(await _propertyService.GetAllAsync(User));

        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> Search(string? location, decimal? minPrice, decimal? maxPrice, int? propertyType) =>
           Ok(await _propertyService.SearchAsync(location, minPrice, maxPrice, propertyType, User));

    }

}
