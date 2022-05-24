using Marketplace.Exceptions;
using Marketplace.Repositories;
using Marketplace.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
	[Authorize(Roles = "Administrator")]
	public class AdminController : Controller
	{
		private readonly IUserRepository userRepository;

		public AdminController(IUserRepository userRepository)
		{
			this.userRepository = userRepository;
		}

		[HttpGet]
		public async Task<IActionResult> Moderators()
		{
			return View(new ModeratorsViewModel(await userRepository.GetModerators()));
		}

		[HttpPost]
		public async Task<IActionResult> Moderators(ModeratorsViewModel model)
		{
			if (ModelState.IsValid)
				try
				{
					await userRepository.AddModerator(model.Register);
					return RedirectToAction("Moderators");
				}
				catch (ModelException e)
				{
					foreach (var error in e.Message.Split(Environment.NewLine))
						ModelState.AddModelError(string.Empty, error);
				}
			model.Moderators = await userRepository.GetModerators();
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Remove(int id)
		{
			try
			{
				await userRepository.RemoveModerator(id);
			}
			catch { }
			return RedirectToAction("Moderators");
		}
	}
}
