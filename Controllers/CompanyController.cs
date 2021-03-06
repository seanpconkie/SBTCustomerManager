﻿using System;
using System.Text;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
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

        [TempData]
        public string StatusMessage { get; set; }

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

            if (viewModel.CompanyContact != null)
            {
                var address = new StringBuilder();

                if (viewModel.CompanyContact.UserId != "" && viewModel.CompanyContact.UserId != null)
                {
                    address.AppendLine(viewModel.CompanyContact.Name);
                }
                address.AppendLine(viewModel.CompanyDetails.Name);
                if (viewModel.CompanyContact.UserContact.AddressLine1 != "" && viewModel.CompanyContact.UserContact.AddressLine1 != null)
                {
                    address.AppendLine(viewModel.CompanyContact.UserContact.AddressLine1);
                }
                if (viewModel.CompanyContact.UserContact.AddressLine2 != "" && viewModel.CompanyContact.UserContact.AddressLine2 != null)
                {
                    address.AppendLine(viewModel.CompanyContact.UserContact.AddressLine2);
                }
                if (viewModel.CompanyContact.UserContact.PostTown != "" && viewModel.CompanyContact.UserContact.PostTown != null)
                {
                    address.AppendLine(viewModel.CompanyContact.UserContact.PostTown);
                }
                if (viewModel.CompanyContact.UserContact.County != "" && viewModel.CompanyContact.UserContact.County != null)
                {
                    address.AppendLine(viewModel.CompanyContact.UserContact.County);
                }
                if (viewModel.CompanyContact.UserContact.Postcode != "" && viewModel.CompanyContact.UserContact.Postcode != null)
                {
                    address.AppendLine(viewModel.CompanyContact.UserContact.Postcode);
                }
                if (viewModel.CompanyContact.UserContact.Country != "" && viewModel.CompanyContact.UserContact.Country != null)
                {
                    address.AppendLine(viewModel.CompanyContact.UserContact.Country);
                }

                viewModel.Address = address.ToString();
            }
            else
            {
                viewModel.Address = viewModel.CompanyDetails.Name;
            }

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
                CompanyUsers = _context.UserDetails.Include(c => c.Profile).Include(c => c.UserContact).Where(c => c.CompanyId == userDetail.CompanyId).Where(c => c.UserId != userDetail.UserId).Where(x => x.EndDate > DateTime.Now)
            };

            viewModel.CompanyContact = _context.UserDetails.Include(c => c.UserContact).SingleOrDefault(c => c.UserId == viewModel.CompanyDetails.UserId);

            return View(viewModel);

        }

        public async Task<IActionResult> DeleteStaff(string userId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var userDetail = _context.UserDetails.SingleOrDefault(c => c.UserId == user.Id);

            var staff = await _userManager.FindByIdAsync(userId);
            if (staff == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var staffDetails = _context.UserDetails.SingleOrDefault(x => x.UserId == userId);
            var result = await _userManager.DeleteAsync(staff);

            if (result.Succeeded)
            {
                staffDetails.EndDate = DateTime.Now;

                _context.Update(staffDetails);

                var contactCompany = _context.CompanyDetails.SingleOrDefault(x => x.UserId == staff.Id);

                if (contactCompany != null)
                {
                    contactCompany.UserId = null;
                    _context.Update(contactCompany);
                }

                _context.SaveChanges();
                StatusMessage = string.Format("User '{0}' has been deleted.", staffDetails.Name);
            }
            else
            {
                StatusMessage = string.Format("Could not delete user '{0}'.", staffDetails.Name);
            }

            AddErrors(result);

            var viewModel = new CompanyViewModel
            {
                CompanyDetails = _context.CompanyDetails.SingleOrDefault(c => c.Id == userDetail.CompanyId),
                CompanyUsers = _context.UserDetails.Include(c => c.Profile).Include(c => c.UserContact).Where(c => c.CompanyId == userDetail.CompanyId).Where(c => c.UserId != userDetail.UserId).Where(x => x.EndDate > DateTime.Now)
            };

            viewModel.CompanyContact = _context.UserDetails.Include(c => c.UserContact).SingleOrDefault(c => c.UserId == viewModel.CompanyDetails.UserId);

            viewModel.StatusMessage = StatusMessage;
            return View("Users",viewModel);

        }

        [HttpGet]
        public async Task<IActionResult> Subscriptions()
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
                CompanySubscriptions = _context.Subscriptions.Include(c => c.SubscriptionType).Where(c => c.CompanyId == userDetail.CompanyId).Where(d => d.EndDate > DateTime.Now).ToList()
            };

            viewModel.CompanyContact = _context.UserDetails.Include(c => c.UserContact).SingleOrDefault(c => c.UserId == viewModel.CompanyDetails.UserId);

            return View(viewModel);

        }


        [HttpGet]
        public IActionResult AddUser(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var viewModel = new RegisterViewModel();

            viewModel.Titles = Title.GetTitles(_context.Titles.Where(c => c.Id > 0).OrderBy(x => x.Value).ToList());

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var createdBy = await _userManager.GetUserAsync(User);
                if (createdBy == null)
                {
                    throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }

                var createdByUserDetail = _context.UserDetails.SingleOrDefault(c => c.UserId == createdBy.Id);

                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                    // create additional user info
                    var contact = new UserContact();
                    var newUser = new UserDetail();
                    long companyId = createdByUserDetail.CompanyId;

                    contact.BuildingNumber = model.BuildingNumber;
                    contact.AddressLine1 = model.AddressLine1;
                    contact.AddressLine2 = model.AddressLine2;
                    contact.PostTown = model.PostTown;
                    contact.County = model.County;
                    contact.Country = model.Country;
                    contact.Postcode = model.Postcode;
                    contact.MobilePhone = model.MobilePhone;
                    contact.WorkPhone = model.WorkPhone;
                    contact.OtherPhone = model.OtherPhone;
                    contact.Email = model.Email;
                    contact.UserId = user.Id;

                    _context.Add(contact);
                    _context.SaveChanges();

                    newUser.ForeName = model.ForeName;
                    newUser.Surname = model.Surname;
                    newUser.Name = model.ForeName + ' ' + model.Surname;
                    newUser.Title = model.Title;
                    newUser.StartDate = DateTime.Now;
                    newUser.EndDate = new DateTime(2070, 1, 1);

                    newUser.UserContactId = contact.Id;
                    newUser.CompanyId = companyId;
                    newUser.UserId = user.Id;

                    _context.Add(newUser);
                    _context.SaveChanges();

                    // Create welcome message
                    _context.Add(Messages.WelcomeMessage(user.Id, newUser.Name));
                    _context.SaveChanges();

                    // Add CanViewCompany role
                    var roleName = _context.Roles.SingleOrDefault(c => c.Name == "CanViewCompany");
                    await _userManager.AddToRoleAsync(user, roleName.Name);

                    // Re-Set user password
                    var passwordCode = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordCallbackUrl = Url.ResetPasswordCallbackLink(user.Id, passwordCode, Request.Scheme);
                    await _emailSender.SendPasswordReset(model.Email, passwordCallbackUrl);

                    return RedirectToAction(nameof(CompanyController.Users), "Company");

                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            model.Titles = Title.GetTitles(_context.Titles.Where(c => c.Id > 0).OrderBy(x => x.Value).ToList());
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
