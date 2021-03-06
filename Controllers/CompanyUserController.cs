﻿using System;
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
using SBTCustomerManager.Models.RoleViewModels;
using SBTCustomerManager.Models.UserDataModels;
using SBTCustomerManager.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SBTCustomerManager.Controllers
{

    [Authorize]
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

            var model = LoadCompanyViewModel(id);

            if (model.CompanyDetails.UserId == model.UserDetails.UserId)
            {
                model = SetCompanyContact(model);
            }

            return View(model);

        }

        [HttpGet]
        public IActionResult UserDetail(long id)
        {

            var model = LoadCompanyViewModel(id);

            if (model.CompanyDetails.UserId == model.UserDetails.UserId)
            {
                model = SetCompanyContact(model);
            }

            return View(model);

        }

        [HttpGet]
        public IActionResult UserProfile(long id)
        {

            var model = LoadCompanyViewModel(id);

            if (model.CompanyDetails.UserId == model.UserDetails.UserId)
            {
                model = SetCompanyContact(model);
            }

            // set role list
            model.RoleList = SetRoleList(model.UserDetails.UserId).Result;

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserDetail(CompanyUserViewModel model)
        {
            // Check company contact
            if (model.IsCompanyContact == true && model.CompanyDetails.UserId != model.UserDetails.UserId)
            {
                CompanyDetail companyDetail = _context.CompanyDetails.SingleOrDefault(c => c.Id == model.CompanyDetails.Id);

                model = SetCompanyContact(model);

                companyDetail.UserId = model.CompanyDetails.UserId;

                _context.SaveChanges();

                StatusMessage = "Company contact has been updated";
            }
            else if (model.IsCompanyContact == false && model.CompanyDetails.UserId == model.UserDetails.UserId)
            {
                StatusMessage = "There must be a user set as company contact.\nSet a different user as company contact to remove this user.";
            }

            // update role details
            UserDetail userDetail = _context.UserDetails.SingleOrDefault(c => c.UserId == model.UserDetails.UserId);
            if (userDetail.ProfileId != model.UserDetails.Profile.Id)
            {
                userDetail.ProfileId = model.UserDetails.Profile.Id;

                _context.SaveChanges();

                StatusMessage = "User role has been updated";
            }

            return RedirectToAction(nameof(UserDetail));
        }


        public async Task<IActionResult> DeleteRole(string roleId, string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // update profile details
            var setResult = await _userManager.RemoveFromRoleAsync(user, _context.Roles.SingleOrDefault(c => c.Id == roleId).Name);
            if (!setResult.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred while removing role '{roleId}' from user with ID '{user.Id}'.");
            }

            return RedirectToAction("UserProfile", new { id = _context.UserDetails.SingleOrDefault(c => c.UserId == userId).Id });

        }

        public async Task<IActionResult> AddRole(string roleId, string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // update profile details
            var setResult = await _userManager.AddToRoleAsync(user, _context.Roles.SingleOrDefault(c => c.Id == roleId).Name);
            if (!setResult.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred adding role '{roleId}' from user with ID '{user.Id}'.");
            }

            StatusMessage = "User profile has been updated";

            return RedirectToAction("UserProfile", new {id = _context.UserDetails.SingleOrDefault(c => c.UserId == userId).Id});

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

        #region Methods
        [NonAction]
        public CompanyUserViewModel SetCompanyContact(CompanyUserViewModel model)
        {
            model.IsCompanyContact = true;

            model.CompanyDetails.UserId = model.UserDetails.UserId;

            return model;

        }
        [NonAction]
        public CompanyUserViewModel LoadCompanyViewModel(long id)
        {

            var model = new CompanyUserViewModel
            {
                StatusMessage = StatusMessage
            };
            List<SelectListItem> userRoles = new List<SelectListItem>();
            var resultList = _context.Profiles.ToList();

            foreach (var item in resultList)
            {
                userRoles.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Role });
            }

            model.UserRoles = userRoles;

            model.UserDetails = _context.UserDetails.Include(c => c.UserContact).SingleOrDefault(c => c.Id == id);
            model.CompanyDetails = _context.CompanyDetails.SingleOrDefault(c => c.Id == model.UserDetails.CompanyId);

            return model;

        }
        [NonAction]
        public async Task<List<RoleDetail>> SetRoleList(string userId)
        {
            var user = await _userManager.GetUserAsync(User);
            var userDetail = _context.UserDetails.SingleOrDefault(x => x.UserId == user.Id);
            List<RoleDetail> outputList = new List<RoleDetail>();
            var roleList = _context.Roles.OrderBy(x => x.Name).ToList();

            foreach (var role in roleList)
            {
                var newRole = new RoleDetail();
                var activeRoles = _context.UserRoles.Where(c => c.UserId == userId).Where(c => c.RoleId == role.Id).ToList();

                newRole.RoleId = role.Id;
                newRole.Name = role.Name;
                newRole.TypeId = _context.RoleTypes.SingleOrDefault(c => c.RoleId == role.Id).Id;
                newRole.Type = _context.RoleType.SingleOrDefault(c => c.Id == newRole.TypeId);
                newRole.Description = _context.RoleDescriptions.SingleOrDefault(c => c.RoleId == newRole.RoleId).Description;

                if (newRole.TypeId == 0 && userDetail.CompanyId != 1)
                {
                    break;
                }

                if (activeRoles.Count == 0)
                {
                    newRole.IsSelected = false;
                }
                else
                {
                    newRole.IsSelected = true;
                }

                outputList.Add(newRole);

            }

            return outputList;

        }
        #endregion

    }
}
