using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SBTCustomerManager.Data;
using SBTCustomerManager.Models;
using SBTCustomerManager.Models.AccountViewModels;
using SBTCustomerManager.Models.CompanyDataModel;
using SBTCustomerManager.Models.SubscriptionViewModels;
using SBTCustomerManager.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SBTCustomerManager.Controllers
{
    [Authorize]
    public class SubscriptionController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public SubscriptionController(
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

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            var viewModel = new SubscriptionViewModel {StatusMessage = StatusMessage};

            viewModel.Types = _context.SubscriptionTypes.Where(c => c.Id > 0);

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(SubscriptionViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                if(model.SubscriptionType.Id == 0)
                {
                    List<SubscriptionType> types = _context.SubscriptionTypes.ToList();

                    if (types.IndexOf(model.SubscriptionType) == -1)
                    {

                        _context.SubscriptionTypes.Add(model.SubscriptionType);
                        _context.SaveChanges();

                        StatusMessage = model.SubscriptionType.Type + " added to Subscription Types.";

                    }
                    else
                    {
                        StatusMessage = model.SubscriptionType.Type + " already exists.";
                    }
                }
                else
                {
                    var typeInDb = _context.SubscriptionTypes.SingleOrDefault(c => c.Id == model.SubscriptionType.Id);

                    typeInDb.Type = model.SubscriptionType.Type;

                    StatusMessage = model.SubscriptionType.Type + " updated.";

                    _context.Update(typeInDb);
                    _context.SaveChanges();
                }

                model.StatusMessage = StatusMessage;
                model.Types = _context.SubscriptionTypes.Where(c => c.Id > 0);

                return View(model);

            }

            // If we got this far, something failed, redisplay form
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
