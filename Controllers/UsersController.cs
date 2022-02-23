using System.Globalization;
using Marketplace.Models;
using Marketplace.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public partial class UsersController : ControllerBase
	{
		private MarketplaceContext db;
		private UserManager<User> userManager;

		public UsersController(MarketplaceContext db, UserManager<User> userManager)
		{
			this.db = db;
			this.userManager = userManager;
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			int languageId = (await db.Languages
				.Where(x => x.Code == CultureInfo.CurrentUICulture.ToString())
				.FirstOrDefaultAsync())?.Id ?? 1;

			User user = await userManager.GetUserAsync(User);
			db.Entry(user).Reference(x => x.City).Load();
			if (user.City != null)
			{
				db.Entry(user.City).Collection(x => x.Names).Load();
				db.Entry(user.City).Reference(x => x.Region).Load();
				db.Entry(user.City.Region).Collection(x => x.Names).Load();
				db.Entry(user.City.Region).Reference(x => x.Country).Load();
				db.Entry(user.City.Region.Country).Collection(x => x.Names).Load();
			}
			return Ok(new UserModel() {
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				Name = user.Name,
				City = user.City == null ? null : new CityModel() {
					Id = user.City.Id,
					Name = user.City.Names.Where(n => n.LanguageId == languageId).First().Value,
					Region = new RegionModel() {
						Id = user.City.Region.Id,
						Name = user.City.Region.Names.Where(n => n.LanguageId == languageId).First().Value,
						Country = new CountryModel() {
							Id = user.City.Region.Country.Id,
							Name = user.City.Region.Country.Names.Where(n => n.LanguageId == languageId).First().Value
						}
					}
				}
			});
		}

		[HttpPut]
		public async Task<IActionResult> Put(UserModel user)
		{
			var result = await userManager.CreateAsync(
				new User() {
					UserName = user.Email,
					Email = user.Email,
					PhoneNumber = user.PhoneNumber,
					Name = user.Name,
					Created = DateTime.UtcNow,
					CityId = user.City?.Id
				},
				user.Password
			);
			return result.Succeeded ? Ok() : BadRequest();
		}
	}
}
