using Marketplace.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class LocationsController : ControllerBase
	{
		private readonly ILocationRepository locationRepository;

		public LocationsController(ILocationRepository locationRepository)
		{
			this.locationRepository = locationRepository;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await locationRepository.GetCountriesAsync());
		}
	}
}
