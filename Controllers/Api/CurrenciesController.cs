using Marketplace.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public partial class CurrenciesController : ControllerBase
	{
		private readonly ICurrencyRepository currencyRepository;

		public CurrenciesController(ICurrencyRepository currencyRepository)
		{
			this.currencyRepository = currencyRepository;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await currencyRepository.GetCurrenciesAsync());
		}
	}
}
