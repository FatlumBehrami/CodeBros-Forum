using CodeBrosForum.Data;
using CodeBrosForum.Data.Models;
using CodeBrosForum.Models.ApplicationUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CodeBrosForum.Models;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CodeBrosForum.Controllers
{
	[Authorize]
    public class ProfileController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IApplicationUser _userService;
	
		public ProfileController(UserManager<ApplicationUser> userManager, IApplicationUser userService)
		{
			_userManager = userManager;
			_userService = userService;
		}

		public IActionResult Detail(string id)
		{
			ApplicationUser user = _userService.GetById(id);

			if(user == null)
            {
				return RedirectToAction("Error");
            }
			
			var userRoles = _userManager.GetRolesAsync(user).Result;
			
			ProfileModel model = new ProfileModel()
			{
				UserId = user.Id,
				UserName = user.UserName,
				UserRating = user.Rating.ToString(),
				Email = user.Email,
				ProfileImageUrl = user.ProfileImageUrl,
				MemberSince = user.MemberSince,
				IsAdmin = userRoles.Contains("Admin")
			};
			return View(model);
		}

		[Authorize(Roles = "Admin")]
		public IActionResult Index()
        {
			var profiles = _userService.GetAll().OrderByDescending(user => user.MemberSince).Select(profile => new ProfileModel
            {
				UserId = profile.Id,
				Email = profile.Email,
				UserName = profile.UserName,
				ProfileImageUrl = profile.ProfileImageUrl,
				UserRating = profile.Rating.ToString(),
				MemberSince = profile.MemberSince
            });

			var model = new ProfileListModel()
			{
				Profiles = profiles
			};

			return View(model);
        }

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
