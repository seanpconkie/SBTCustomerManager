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
using SBTCustomerManager.Models.SiteSettingsViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SBTCustomerManager.Controllers
{
    public class SiteSettingsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public SiteSettingsController(
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
            var viewModel = new SiteSettingsViewModel() { StatusMessage = StatusMessage };

            // Add Titles
            viewModel.Titles = _context.Titles.Where(c => c.Id > 0).OrderBy(x => x.Value).ToList();
            // Add Profiles (= position)
            viewModel.Profiles = _context.Profiles.Where(cw => cw.Id > 0).OrderBy(x => x.Role).ToList();
            // Add role types
            viewModel.RoleTypes = _context.RoleType.Where(cw => cw.Id > 0).OrderBy(x => x.Type).ToList();

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(SiteSettingsViewModel model)
        {
            bool isValueDuplicate = false;
            try
            {
                switch (model.UpdateType)
                {
                    case "Profile":
                        var newProfile = new Profile { Role = model.UpdateValue };
                        if (model.UpdateId == 0)
                        {
                            List<Profile> profileList = _context.Profiles.ToList();

                            foreach (var profile in profileList)
                            {
                                if (profile.Role == model.UpdateValue)
                                {
                                    isValueDuplicate = true;
                                    break;
                                }
                            }
                            if (!isValueDuplicate)
                            {
                                _context.Add(newProfile);
                            }
                            else
                            {
                                throw new ApplicationException(string.Format("{0} already exists", model.UpdateValue));
                            }
                        }
                        else
                        {
                            var profileInDb = _context.Profiles.SingleOrDefault(c => c.Id == model.UpdateId);
                            if (profileInDb == null)
                            {
                                throw new ApplicationException($"Unable to locate Profile id '{model.UpdateId}'");
                            }

                            profileInDb.Role = model.UpdateValue;
                            _context.Update(profileInDb);
                        }
                        break;
                    case "Title":
                        var newTitle = new Title { Value = model.UpdateValue };
                        if (model.UpdateId == 0)
                        {
                            List<Title> titleList = _context.Titles.ToList();
                            foreach (var title in titleList)
                            {
                                if (title.Value == model.UpdateValue)
                                {
                                    isValueDuplicate = true;
                                    break;
                                }
                            }
                            if (!isValueDuplicate)
                            {
                                _context.Add(newTitle);
                            }
                            else
                            {
                                throw new ApplicationException(string.Format("{0} already exists", model.UpdateValue));
                            }
                        }
                        else
                        {
                            var titleInDb = _context.Titles.SingleOrDefault(c => c.Id == model.UpdateId);
                            if (titleInDb == null)
                            {
                                throw new ApplicationException($"Unable to locate Title id '{model.UpdateId}'");
                            }

                            titleInDb.Value = model.UpdateValue;
                            _context.Update(titleInDb);
                        }
                        break;
                    case "RoleType":
                        var newRoleType = new RoleType { Type = model.UpdateValue };
                        if (model.UpdateId == 0)
                        {
                            List<RoleType> roleTypelist = _context.RoleType.ToList();
                            foreach (var role in roleTypelist)
                            {
                                if (role.Type == model.UpdateValue)
                                {
                                    isValueDuplicate = true;
                                    break;
                                }
                            }
                            if (!isValueDuplicate)
                            {
                                _context.Add(newRoleType);
                            }
                            else
                            {
                                throw new ApplicationException(string.Format("{0} already exists", model.UpdateValue));
                            }
                        }
                                else
                                {
                            var roleTypeInDb = _context.RoleType.SingleOrDefault(c => c.Id == model.UpdateId);
                            if (roleTypeInDb == null)
                            {
                                throw new ApplicationException($"Unable to locate Role Type id '{model.UpdateId}'");
                            }

                            roleTypeInDb.Type = model.UpdateValue;
                            _context.Update(roleTypeInDb);
                        }
                        break;
                    default:
                        break;
                }
                _context.SaveChanges();
                StatusMessage = string.Format("New {0} added - {1}", model.UpdateType, model.UpdateValue);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("New {0} could not be added - error details '{1}'", model.UpdateType, ex.Message);
            }

            var viewModel = new SiteSettingsViewModel() { StatusMessage = StatusMessage };

            // Add Titles
            viewModel.Titles = _context.Titles.Where(c => c.Id > 0).OrderBy(x => x.Value).ToList();
            // Add Profiles (= position)
            viewModel.Profiles = _context.Profiles.Where(cw => cw.Id > 0).OrderBy(x => x.Role).ToList();
            // Add role types
            viewModel.RoleTypes = _context.RoleType.Where(cw => cw.Id > 0).OrderBy(x => x.Type).ToList();

            return View(viewModel);
        }
        [HttpPut]
        public IActionResult Delete(long id, string updateType)
        {
            try
            {
                if (id == 0)
                {
                    throw new ApplicationException("No Id supplied.");
                }

                switch (updateType)
                {
                    case "Profile":
                        var profile = new Profile { Id = id };
                        var userProfileList = _context.UserDetails.Where(x => x.ProfileId == id);
                        foreach (var user in userProfileList)
                        {
                            user.ProfileId = 0;
                            _context.Update(user);
                        }
                        _context.Remove(profile);
                        break;
                    case "Title":
                        var title = new Title { Id = id };
                        var userTitleList = _context.UserDetails.Where(x => x.Title == _context.Titles.SingleOrDefault(y => y.Id == id).Value);
                        foreach (var user in userTitleList)
                        {
                            user.Title = "Unknown";
                            _context.Update(user);
                        }
                        _context.Remove(title);
                        break;
                    case "RoleType":
                        var roleType = new RoleType { Id = (int)id };
                        var roleList = _context.RoleTypes.Where(cw => cw.Id == id);
                        foreach (var role in roleList)
                        {
                            role.Id = 0;
                            _context.Update(role);

                        }
                        _context.Remove(roleType);
                        break;
                    default:
                        break;
                }
                _context.SaveChanges();
                StatusMessage = string.Format("Id '{0}' deleted.", id);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Id '{0}' could not be deleted - error details '{1}'", id, ex.Message);
            }

            var viewModel = new SiteSettingsViewModel() { StatusMessage = StatusMessage };

            // Add Titles
            viewModel.Titles = _context.Titles.Where(c => c.Id > 0).OrderBy(x => x.Value).ToList();
            // Add Profiles (= position)
            viewModel.Profiles = _context.Profiles.Where(cw => cw.Id > 0).OrderBy(x => x.Role).ToList();
            // Add role types
            viewModel.RoleTypes = _context.RoleType.Where(cw => cw.Id > 0).OrderBy(x => x.Type).ToList();

            return View("Index",viewModel);
        }

    }
}
