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
    [Authorize]
    [Route("[controller]/[action]")]
    public class CompanyController : Controller
    {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly IEmailSender _emailSender;
            private readonly ILogger _logger;
            private readonly ApplicationDbContext _context;

            public CompanyController(
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


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userDetail = _context.UserDetails.SingleOrDefault(c => c.UserId == user.Id);

            var viewModel = new CompanyViewModel
            {
                CompanyDetails = _context.CompanyDetails.SingleOrDefault(c => c.Id == userDetail.CompanyId)
            };

            viewModel.CompanyContact = _context.UserDetails.Include(c => c.UserContact).SingleOrDefault(c => c.UserId == viewModel.CompanyDetails.UserId);

            return View(viewModel);

        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userDetail = _context.UserDetails.SingleOrDefault(c => c.UserId == user.Id);

            var viewModel = new CompanyViewModel
            {
                CompanyDetails = _context.CompanyDetails.SingleOrDefault(c => c.Id == userDetail.CompanyId),
                CompanyUsers = _context.UserDetails.Where(c => c.CompanyId == userDetail.CompanyId)
            };

            viewModel.CompanyContact = _context.UserDetails.Include(c => c.UserContact).SingleOrDefault(c => c.UserId == viewModel.CompanyDetails.UserId);

            return View(viewModel);

        }
    }
}
