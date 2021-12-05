using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Marketplace.Models;
using Microsoft.AspNetCore.Authorization;

namespace Marketplace.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public partial class UsersController : ControllerBase
	{
		private UserManager<User> userManager;
		private JwtConfiguration jwtConfig;

		public UsersController(UserManager<User> userManager, IOptions<JwtConfiguration> jwtOptions)
		{
			this.userManager = userManager;
			jwtConfig = jwtOptions.Value;
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

		[HttpPost]
		public async Task<IActionResult> Post(UserViewModel user)
		{
			var identity = await GetIdentity(user.Email, user.Password);
			if (identity == null)
				return BadRequest();

			var now = DateTime.UtcNow;
			var jwt = new JwtSecurityToken(
				issuer: jwtConfig.Issuer,
				audience: jwtConfig.Audience,
				notBefore: now,
				claims: identity.Claims,
				expires: now.Add(jwtConfig.GetTimeSpanLifetime()),
				signingCredentials: new SigningCredentials(jwtConfig.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
			);
			var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

			return Ok(new {
				accessToken = encodedJwt
			});
		}

		[HttpPut]
		public async Task<IActionResult> Put(UserViewModel user)
		{
			var result = await userManager.CreateAsync(new User() {
					UserName = user.Email,
					Email = user.Email,
					PhoneNumber = user.PhoneNumber,
					Name = user.Name
				},
				user.Password
			);
			return result.Succeeded ? Ok() : BadRequest();
		}

		private async Task<ClaimsIdentity?> GetIdentity(string? userName, string? password)
		{
			User user = await userManager.FindByNameAsync(userName);
			if (user == null)
				return null;
			var results = await Task.WhenAll(
				userManager.PasswordValidators.Select(x => x.ValidateAsync(userManager, user, password))
			);
			if (!results.All(x => x.Succeeded))
				return null;

			var claims = new[] {
				new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
				new Claim(ClaimsIdentity.DefaultRoleClaimType, string.Join(" ", await userManager.GetRolesAsync(user))),
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
			};
			return new ClaimsIdentity(claims, "token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
		}
	}
}
