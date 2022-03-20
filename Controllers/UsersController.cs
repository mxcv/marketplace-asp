using System.Globalization;
using Marketplace.Dto;
using Marketplace.Models;
using Marketplace.ViewModels;
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
		private MarketplaceDbContext db;
		private UserManager<User> userManager;

		public UsersController(MarketplaceDbContext db, UserManager<User> userManager)
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
			db.Entry(user).Reference(x => x.Image).Load();
			if (user.Image != null)
				db.Entry(user.Image).Reference(x => x.File).Load();

			return Ok(new UserDto() {
				Id = user.Id,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				Name = user.Name,
				Created = user.Created,
				City = user.City == null ? null : new CityDto() {
					Id = user.City.Id,
					Name = user.City.Names.Where(n => n.LanguageId == languageId).First().Value,
					Region = new RegionDto() {
						Id = user.City.Region.Id,
						Name = user.City.Region.Names.Where(n => n.LanguageId == languageId).First().Value,
						Country = new CountryDto() {
							Id = user.City.Region.Country.Id,
							Name = user.City.Region.Country.Names.Where(n => n.LanguageId == languageId).First().Value
						}
					}
				},
				Image = user.Image == null ? null : new ImageDto() {
					Path = string.Format("/{0}/{1}", ImagesController.DirectoryPath, user.Image.File.Name)
				}
			});
		}

		[HttpPost]
		public async Task<IActionResult> Post(UserRegisterViewModel userRegisterModel)
		{
			User user = new User() {
				UserName = userRegisterModel.Email,
				Email = userRegisterModel.Email,
				PhoneNumber = userRegisterModel.PhoneNumber,
				Name = userRegisterModel.Name,
				Created = DateTime.UtcNow,
				CityId = userRegisterModel.City?.Id
			};
			var result = await userManager.CreateAsync(user, userRegisterModel.Password);
			return result.Succeeded ? Ok(user.Id) : BadRequest();
		}
	}
}
