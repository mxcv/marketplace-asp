using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Marketplace.Models;
using Marketplace.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Marketplace.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public partial class TokensController : ControllerBase
	{
		private MarketplaceContext db;
		private UserManager<User> userManager;
		private JwtConfiguration jwtConfig;

		public TokensController(MarketplaceContext db, UserManager<User> userManager, IOptions<JwtConfiguration> jwtOptions)
		{
			this.db = db;
			this.userManager = userManager;
			jwtConfig = jwtOptions.Value;
		}

		[HttpPost]
		public async Task<IActionResult> Access(UserLoginModel userLogin)
		{
			var identity = await GetIdentity(userLogin.Email, userLogin.Password);
			if (identity == null)
				return BadRequest();

			User user = await userManager.FindByNameAsync(identity.Name);
			return Ok(await Refresh(user, identity.Claims));
		}

		[HttpPost]
		public async Task<IActionResult> Refresh(JwtModel jwt)
		{
			var principal = GetPrincipalFromExpiredToken(jwt.AccessToken);
			if (principal == null || principal.Identity == null)
				return BadRequest();

			User user = await userManager.FindByNameAsync(principal.Identity.Name);
			await db.Entry(user).Reference(x => x.RefreshToken).LoadAsync();
			if (user.RefreshToken == null
				|| user.RefreshToken.Token != jwt.RefreshToken
				|| user.RefreshToken.Expired < DateTime.UtcNow)
				return BadRequest();

			return Ok(await Refresh(user, principal.Claims));
		}

		private async Task<JwtModel> Refresh(User user, IEnumerable<Claim> claims)
		{
			var oldRefreshToken = db.RefreshTokens.Where(x => x.UserId == user.Id).FirstOrDefault();
			if (oldRefreshToken != null)
				db.RefreshTokens.Remove(oldRefreshToken);

			var jwt = new JwtModel {
				AccessToken = GenerateAccessToken(claims),
				RefreshToken = GenerateRefreshToken()
			};
			user.RefreshToken = new RefreshToken() {
				Token = jwt.RefreshToken,
				Expired = DateTime.UtcNow.Add(jwtConfig.RefreshTimeSpan)
			};
			await db.SaveChangesAsync();
			return jwt;
		}

		private async Task<ClaimsIdentity?> GetIdentity(string userName, string password)
		{
			User user = await userManager.FindByNameAsync(userName);
			if (user == null)
				return null;

			if (userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Failed)
				return null;

			var claims = new[] {
				new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
				new Claim(ClaimsIdentity.DefaultRoleClaimType, string.Join(" ", await userManager.GetRolesAsync(user))),
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
			};
			return new ClaimsIdentity(claims, "token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
		}

		private string GenerateAccessToken(IEnumerable<Claim> claims)
		{
			var now = DateTime.UtcNow;
			var jwt = new JwtSecurityToken(
				issuer: jwtConfig.Issuer,
				audience: jwtConfig.Audience,
				notBefore: now,
				claims: claims,
				expires: now.Add(jwtConfig.AccessTimeSpan),
				signingCredentials: new SigningCredentials(jwtConfig.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256)
			);
			return new JwtSecurityTokenHandler().WriteToken(jwt);
		}

		private string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);
				return Convert.ToBase64String(randomNumber);
			}
		}

		private ClaimsPrincipal? GetPrincipalFromExpiredToken(string accessToken)
		{
			var tokenValidationParameters = new TokenValidationParameters {
				ValidateAudience = true,
				ValidateIssuer = true,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = jwtConfig.SymmetricSecurityKey,
				ValidateLifetime = false,
				ValidAudience = jwtConfig.Audience,
				ValidIssuer = jwtConfig.Issuer
			};

			try
			{
				var principal = new JwtSecurityTokenHandler()
					.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);

				if (securityToken == null || !((JwtSecurityToken)securityToken).Header.Alg
				.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
					return null;

				return principal;
			}
			catch (SecurityTokenException)
			{
				return null;
			}
		}
	}
}
