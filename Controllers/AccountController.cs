using Marketplace.Dto;
using Marketplace.Exceptions;
using Marketplace.Models;
using Marketplace.Repositories;
using Marketplace.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Marketplace.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<User> signInManager;
		private readonly IUserRepository userRepository;
		private readonly IStringLocalizer<AccountController> localizer;

		public AccountController(SignInManager<User> signInManager, IUserRepository userRepository, IStringLocalizer<AccountController> localizer)
		{
			this.signInManager = signInManager;
			this.userRepository = userRepository;
			this.localizer = localizer;
		}

		[HttpGet]
		public IActionResult Login(string returnUrl)
		{
			return View(new LoginViewModel { ReturnUrl = returnUrl });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
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
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (model.CityId != null)
						model.City = new CityDto() { Id = model.CityId.Value };

					if (model.Image == null)
						await userRepository.AddSeller(model);
					else
						await userRepository.AddSeller(model, model.Image);

					return RedirectToAction("Index", "Home");
				}
				catch (ModelException e)
				{
					foreach (var error in e.Message.Split(Environment.NewLine))
						ModelState.AddModelError(string.Empty, error);
				}
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
