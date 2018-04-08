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
using SBTCustomerManager.Models.RoleViewModels;
using SBTCustomerManager.Models.UserDataModels;
using SBTCustomerManager.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SBTCustomerManager.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public RoleController(
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
            var viewModel = new RoleViewModel { StatusMessage = StatusMessage };

            viewModel.Roles = SetRoleList();

            return View(viewModel);
        }

        
        public IActionResult Delete(string id)
        {

            // Find role users
            var roleUsers = _context.UserRoles.Where(c => c.RoleId == id);
            Result result = Result.Succeed;

            // Remove each user from the role
            foreach (var user in roleUsers)
            {

                var loopResult = RemoveRoleFromUser(user.UserId, id);

                if (loopResult.Result == Result.Fail)
                {
                    StatusMessage = string.Format("User ID '{0}' could not be removed from Role ID '{1}'", user.UserId, id);
                    result = loopResult.Result;
                    break;
                }

            }

            if (result == Result.Succeed)
            {
                // Remove role
                var roleInDb = _context.Roles.SingleOrDefault(c => c.Id == id);
                var roleTypeInDb = _context.RoleTypes.SingleOrDefault(c => c.RoleId == id);
                var roleDescriptionInDb = _context.RoleDescriptions.SingleOrDefault(c => c.RoleId == id);

                try
                {
                    _context.Remove(roleInDb);
                    _context.Remove(roleTypeInDb);
                    _context.Remove(roleDescriptionInDb);
                    _context.SaveChanges();
                }
                catch
                {
                    result = Result.Fail;
                    StatusMessage = string.Format("Unable to delete role ID '{0}'", id);
                }

            }

            var viewModel = new RoleViewModel { StatusMessage = StatusMessage };

            viewModel.Roles = SetRoleList();

            return View("Index", viewModel);

        }


        [HttpGet]
        public IActionResult Add()
        {
            
            var viewModel = new RoleViewModel { StatusMessage = StatusMessage };

            List<SelectListItem> roleTypes = new List<SelectListItem>();
            var resultList = _context.RoleType.ToList();

            foreach (var item in resultList)
            {
                roleTypes.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Type });
            }

            viewModel.RoleTypes = roleTypes;

            return View(viewModel);

        }

        [HttpPost]
        public IActionResult Add(RoleViewModel model)
        {

            var newRole = new IdentityRole { Name = model.Role.Name, NormalizedName = model.Role.Name.ToUpper() };

            try
            {
                _context.Add(newRole);
                _context.SaveChanges();
            }
            catch
            {
                model.StatusMessage = "An error occured.";
                // If we got this far, something failed, redisplay form
                return View(model);
            }

            var createdRole = _context.Roles.SingleOrDefault(c => c.Name == model.Role.Name);

            var roleType = new RoleTypes { RoleId = createdRole.Id, Id = model.Role.Type.Id };
            var roleDescription = new RoleDescription { RoleId = createdRole.Id, Description = model.Role.Description };

            try
            {
                _context.Update(createdRole);
                _context.Add(roleType);
                _context.Add(roleDescription);
                _context.SaveChanges();
            }
            catch
            {
                model.StatusMessage = "An error occured.";
                // If we got this far, something failed, redisplay form
                return View(model);
            }

            return RedirectToLocal("/Role/Index");

        }


        [HttpGet]
        public IActionResult Edit(string id)
        {

            var viewModel = new RoleViewModel { StatusMessage = StatusMessage };

            List<SelectListItem> roleTypes = new List<SelectListItem>();
            var resultList = _context.RoleType.ToList();

            foreach (var item in resultList)
            {
                roleTypes.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Type });
            }

            var roleInDb = _context.Roles.SingleOrDefault(c => c.Id == id);
            var role = new RoleDetail() { RoleId = id };

            role.Name = roleInDb.Name;
            role.TypeId = _context.RoleTypes.SingleOrDefault(c => c.RoleId == id).Id;
            role.Type = _context.RoleType.SingleOrDefault(c => c.Id == role.TypeId);
            role.Description = _context.RoleDescriptions.SingleOrDefault(c => c.RoleId == role.RoleId).Description;

            viewModel.Role = role;
            viewModel.RoleTypes = roleTypes;

            return View(viewModel);

        }

        [HttpPost]
        public IActionResult Edit(RoleViewModel model)
        {

            var identityRoleInDb = _context.Roles.SingleOrDefault(c => c.Id == model.Role.RoleId);
            var roleTypeInDb = _context.RoleTypes.SingleOrDefault(c => c.RoleId == model.Role.RoleId);
            var roleDescriptionInDb = _context.RoleDescriptions.SingleOrDefault(c => c.RoleId == model.Role.RoleId);

            identityRoleInDb.Name = model.Role.Name;
            identityRoleInDb.NormalizedName = model.Role.Name.ToUpper();

            roleTypeInDb.Id = model.Role.Type.Id;

            roleDescriptionInDb.Description = model.Role.Description;

            try
            {
                _context.Update(identityRoleInDb);
                _context.Update(roleTypeInDb);
                _context.Update(roleDescriptionInDb);
                _context.SaveChanges();
            }
            catch
            {
                model.StatusMessage = "An error occured.";
                // If we got this far, something failed, redisplay form
                return View(model);
            }

            return RedirectToLocal("/Role/Index");

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
        private List<RoleDetail> SetRoleList()
        {
            List<RoleDetail> outputList = new List<RoleDetail>();
            var roleList = _context.Roles.ToList();

            foreach (var role in roleList)
            {
                var newRole = new RoleDetail();

                newRole.RoleId = role.Id;
                newRole.Name = role.Name;
                newRole.TypeId = _context.RoleTypes.SingleOrDefault(c => c.RoleId == role.Id).Id;
                newRole.Type = _context.RoleType.SingleOrDefault(c => c.Id == newRole.TypeId);
                newRole.Description = _context.RoleDescriptions.SingleOrDefault(c => c.RoleId == newRole.RoleId).Description;

                outputList.Add(newRole);

            }

            return outputList;

        }

        [NonAction]
        private async Task<Result> RemoveRoleFromUser(string userId, string roleId)
        {
            try
            {

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new ApplicationException($"Unable to load user with ID '{userId}'.");
                }

                var roleName = _context.Roles.SingleOrDefault(c => c.Id == roleId).Name;
                if (roleName == null)
                {
                    throw new NullReferenceException(string.Format("Role Name cannot be NULL, role id: '{0}'", roleId));
                }

                var setResult = await _userManager.RemoveFromRoleAsync(user, roleName);

                return Result.Succeed;

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return Result.Fail;
            
            }
        }
        #endregion
    }
}
