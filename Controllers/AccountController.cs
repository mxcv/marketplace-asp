using Marketplace.Models;
using Marketplace.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
	public class AccountController : Controller
	{
		private SignInManager<User> signInManager;

		public AccountController(SignInManager<User> signInManager)
		{
			this.signInManager = signInManager;
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
					ModelState.AddModelError(string.Empty, "Invalid email or password");
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
