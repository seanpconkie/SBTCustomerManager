using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SBTCustomerManager.Data;
using SBTCustomerManager.Models;
using SBTCustomerManager.Models.AccountViewModels;
using SBTCustomerManager.Models.CompanyDataModel;
using SBTCustomerManager.Models.UserDataModels;
using SBTCustomerManager.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SBTCustomerManager.Controllers
{
    public class CompanyUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public CompanyUserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger,
            ApplicationDbContext context
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public IActionResult UserContact(long id)
        {

            var model = new CompanyUserViewModel
            {
                StatusMessage = StatusMessage
            };

            model.UserDetails = _context.UserDetails.Include(c => c.UserContact).SingleOrDefault(c => c.Id == id);
            model.CompanyDetails = _context.CompanyDetails.SingleOrDefault(c => c.Id == model.UserDetails.CompanyId);

            return View(model);
        }

        [HttpGet]
        public IActionResult UserDetail(long id)
        {

            var model = new CompanyUserViewModel
            {
                StatusMessage = StatusMessage
            };

            model.UserDetails = _context.UserDetails.Include(c => c.UserContact).SingleOrDefault(c => c.Id == id);
            model.CompanyDetails = _context.CompanyDetails.SingleOrDefault(c => c.Id == model.UserDetails.CompanyId);

            if (model.CompanyDetails.UserId != model.UserDetails.UserId)
            {
                model.SetCompanyContact();
            }

            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserDetail(CompanyUserViewModel model)
        {
            if (model.IsCompanyContact == true && model.CompanyDetails.UserId != model.UserDetails.UserId)
            {
                model.SetCompanyContact();
                StatusMessage = "Company contact has been updated";
            }
            else if (model.IsCompanyContact == false && model.CompanyDetails.UserId == model.UserDetails.UserId)
            {
                StatusMessage = "There must be a user set as company contact.\nSet a different user as company contact to remove this user.";
            }

            return RedirectToAction(nameof(UserDetail));
        }


        [HttpGet]
        public async Task<IActionResult> UserProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new CompanyUserViewModel
            {

            };

            return View(model);
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }
        }

        #endregion


    }
}
