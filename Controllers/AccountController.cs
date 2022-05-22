﻿using Marketplace.Models;
using Marketplace.Repositories;
using Marketplace.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Marketplace.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<User> userManager;
		private readonly SignInManager<User> signInManager;
		private readonly IImageRepository imageRepository;
		private readonly IStringLocalizer<AccountController> localizer;

		public AccountController(UserManager<User> userManager,
			SignInManager<User> signInManager,
			IImageRepository imageRepository,
			IStringLocalizer<AccountController> localizer)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.imageRepository = imageRepository;
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
				User user = new User() {
					UserName = model.Email,
					Email = model.Email,
					PhoneNumber = model.PhoneNumber,
					Name = model.Name,
					Created = DateTime.UtcNow,
					CityId = model.CityId
				};
				var result = await userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					if (model.Image != null)
						await imageRepository.SetUserImageAsync(model.Image, user.Id);
					return RedirectToAction("Index", "Home");
				}
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
