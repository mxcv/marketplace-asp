using Marketplace.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
		{
            this.categoryRepository = categoryRepository;
        }

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await categoryRepository.GetCategoriesAsync());
		}
	}
}
