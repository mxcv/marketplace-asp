using Marketplace.Models;
using Marketplace.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Marketplace.Controllers
{
	public class AccountController : Controller
	{
		private UserManager<User> userManager;
		private SignInManager<User> signInManager;
		private IStringLocalizer<AccountController> localizer;

		public AccountController(UserManager<User> userManager,
			SignInManager<User> signInManager,
			IStringLocalizer<AccountController> localizer)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.localizer = localizer;
		}

		[HttpGet]
		public IActionResult Login(string returnUrl)
		{
			return View(new WebLoginViewModel { ReturnUrl = returnUrl });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(WebLoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = await signInManager.PasswordSignInAsync(
					model.Email,
					model.Password,
					model.RememberMe,
					false
				);

				if (result.Succeeded)
					return !string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl)
						? Redirect(model.ReturnUrl)
						: RedirectToAction("Index", "Home");
				else
					ModelState.AddModelError(string.Empty, localizer["LoginError"]);
			}
			return View(model);
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(WebRegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				User user = new User() {
					UserName = model.Email,
					Email = model.Email,
					PhoneNumber = model.PhoneNumber,
					Name = model.Name,
					Created = DateTime.UtcNow,
					CityId = model.City?.Id
				};
				var result = await userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
					return RedirectToAction("Index", "Home");
				else
					foreach (var error in result.Errors)
						ModelState.AddModelError(string.Empty, error.Description);
			}
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}
