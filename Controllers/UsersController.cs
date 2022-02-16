using Marketplace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public partial class UsersController : ControllerBase
	{
		private UserManager<User> userManager;

		public UsersController(UserManager<User> userManager)
		{
			this.userManager = userManager;
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			User user = await userManager.GetUserAsync(User);
			return Ok(new UserViewModel() {
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				Name = user.Name
			});
		}

		[HttpPut]
		public async Task<IActionResult> Put(UserViewModel user)
		{
			var result = await userManager.CreateAsync(
				new User() {
					UserName = user.Email,
					Email = user.Email,
					PhoneNumber = user.PhoneNumber,
					Name = user.Name,
					Created = DateTime.UtcNow
				},
				user.Password
			);
			return result.Succeeded ? Ok() : BadRequest();
		}
	}
}
