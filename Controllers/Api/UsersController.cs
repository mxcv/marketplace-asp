using Marketplace.Repositories;
using Marketplace.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public partial class UsersController : ControllerBase
	{
		private readonly IUserRepository userRepository;

		public UsersController(IUserRepository userRepository)
		{
			this.userRepository = userRepository;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			try
			{
				return Ok(await userRepository.GetUser(id));
			}
			catch
			{
				return BadRequest();
			}
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				return Ok(await userRepository.GetCurrentUser());
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpPost]
		public async Task<IActionResult> Post(ApiRegisterViewModel model)
		{
			try
			{
				return Ok(await userRepository.AddUser(model));
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}
