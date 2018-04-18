using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using SBTCustomerManager.Data;
using SBTCustomerManager.Models;
using SBTCustomerManager.Models.CustomerViewModels;
using SBTCustomerManager.Models.RoleViewModels;
using SBTCustomerManager.Models.UserDataModels;
using SBTCustomerManager.Services;
using SBTCustomerManager.Models.CompanyDataModel;
using SBTCustomerManager.Models.AccountViewModels;
using SBTCustomerManager.Models.SubscriptionViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SBTCustomerManager.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public CustomerController(
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
            var userDetail = _context.UserDetails.SingleOrDefault(x => x.UserId == user.Id);

            var viewModel = new CustomerViewModel() {StatusMessage = StatusMessage};
            var customerList = new List<Customer>();

            var companyList = _context.CompanyDetails.Where(x => x.Id != userDetail.CompanyId);

            foreach (var company in companyList)
            {
                var companyUsers = _context.UserDetails.Where(x => x.CompanyId == company.Id).Include(c => c.UserContact).Include(p => p.Profile).ToList();
                int contactIndex = -1;
                int inc = -1;

                var customer = new Customer()
                {
                    Company = company,
                    Staff = companyUsers
                };

                foreach (var item in companyUsers)
                {
                    inc++;
                    if (item.UserId == company.UserId)
                    {
                        contactIndex = inc;
                    }
                }

                if (contactIndex >= 0)
                {
                    customer.Contact = companyUsers[contactIndex];
                }
                else
                {
                    customer.Contact = new UserDetail() { Name = "Unknown" };
                    customer.Contact.UserContact = new UserContact();
                    customer.Contact.Profile = new Profile() { Id = 0, Role = "Staff" };
                }
                customerList.Add(customer);

            }

            viewModel.Customers = customerList;

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Customer(long id)
        {
            return View(GetSelectedCustomer(id));
        }
        [HttpGet]
        public IActionResult Staff(long id, bool includeDeleted = false)
        {
            return View(GetSelectedCustomer(id, includeDeleted));
        }

        [HttpGet]
        public IActionResult StaffDetail(long id)
        {
            return View(GetStaffMember(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StaffDetail(StaffMemberViewModel model)
        {
            var userInDb = _context.UserDetails.SingleOrDefault(x => x.Id == model.UserDetails.Id);
            var selectedCompany = _context.CompanyDetails.SingleOrDefault(x => x.Id == model.UserDetails.Company.Id);

            userInDb.ForeName = model.UserDetails.ForeName;
            userInDb.Surname = model.UserDetails.Surname;
            userInDb.Name = model.UserDetails.ForeName + " " + model.UserDetails.Surname;
            userInDb.Title = model.UserDetails.Title;
            userInDb.CompanyId = selectedCompany.Id;
            userInDb.ProfileId = model.UserDetails.Profile.Id;

            _context.Update(userInDb);

            if (model.IsCompanyContact && selectedCompany.UserId != model.UserDetails.UserId)
            {
                var companyInDb = _context.CompanyDetails.SingleOrDefault(x => x.Id == model.UserDetails.CompanyId);

                companyInDb.UserId = model.UserDetails.UserId;

                _context.Update(companyInDb);
            }

            _context.SaveChanges();

            return View(GetStaffMember(model.UserDetails.Id));
        }

        [HttpGet]
        public IActionResult StaffContact(long id)
        {
            return View(GetStaffMember(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StaffContact(StaffMemberViewModel model)
        {
            var userContactInDb = _context.UserContacts.SingleOrDefault(x => x.Id == model.UserDetails.UserContactId);
            var user = await _userManager.FindByIdAsync(model.UserDetails.UserId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            userContactInDb.Email = model.UserDetails.UserContact.Email;
            userContactInDb.MobilePhone = model.UserDetails.UserContact.MobilePhone;
            userContactInDb.OtherPhone = model.UserDetails.UserContact.OtherPhone;
            userContactInDb.WorkPhone = model.UserDetails.UserContact.WorkPhone;

            userContactInDb.BuildingNumber = model.UserDetails.UserContact.BuildingNumber;
            userContactInDb.AddressLine1 = model.UserDetails.UserContact.AddressLine1;
            userContactInDb.AddressLine2 = model.UserDetails.UserContact.AddressLine2;
            userContactInDb.PostTown = model.UserDetails.UserContact.PostTown;
            userContactInDb.County = model.UserDetails.UserContact.County;
            userContactInDb.Country = model.UserDetails.UserContact.Country;
            userContactInDb.Postcode = model.UserDetails.UserContact.Postcode;

            _context.Update(userContactInDb);

            var userInDb = _context.Users.SingleOrDefault(x => x.Id == model.UserDetails.UserId);

            var email = user.Email;
            if (model.UserDetails.UserContact.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, model.UserDetails.UserContact.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                }
            }

            _context.SaveChanges();

            return View(GetStaffMember(model.UserDetails.Id));
        }
        [HttpGet]
        public IActionResult StaffProfile(long id)
        {
            var viewModel = GetStaffMember(id);

            viewModel.RoleList = SetRoleList(viewModel.UserDetails.UserId).Result;

            return View(viewModel);
        }


        [HttpGet]
        public IActionResult AddStaff(long id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var viewModel = new AddStaffViewModel();

            var company = _context.CompanyDetails.SingleOrDefault(c => c.Id == id);

            var customer = new Customer()
            {
                Company = company
            };

            viewModel.Customer = customer;
            viewModel.Titles = Title.GetTitles(_context.Titles.Where(c => c.Id > 0).OrderBy(x => x.Value).ToList());

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStaff(AddStaffViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {

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
                    newUser.CompanyId = model.Customer.Company.Id;
                    newUser.UserId = user.Id;

                    _context.Add(newUser);
                    _context.SaveChanges();

                    // Create welcome message
                    _context.Add(Messages.WelcomeMessage(user.Id, newUser.Name));
                    _context.SaveChanges();

                    // Add CanViewCompany role
                    await _userManager.AddToRoleAsync(user, "CanViewCompany");

                    return RedirectToLocal("/Customer/Customer/" + model.Customer.Company.Id.ToString());

                }
                AddErrors(result);
            }


            // If we got this far, something failed, redisplay form
            model.Titles = Title.GetTitles(_context.Titles.Where(c => c.Id > 0).OrderBy(x => x.Value).ToList());
            return View(model);
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

            return RedirectToAction("StaffProfile", new { id = _context.UserDetails.SingleOrDefault(c => c.UserId == userId).Id });

        }

        public async Task<IActionResult> DeleteStaff(string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var userDetails = _context.UserDetails.SingleOrDefault(x => x.UserId == userId);


            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {

                userDetails.EndDate = DateTime.Now;

                _context.Update(userDetails);
                _context.SaveChanges();

                StatusMessage = string.Format("User '{0}' has been deleted.",userDetails.Name);

            }
            else
            {
                StatusMessage = string.Format("Could not delete user '{0}'.", userDetails.Name);
            }
            AddErrors(result);
            var viewModel = GetSelectedCustomer(userDetails.Id);
            viewModel.StatusMessage = StatusMessage;
            return View(viewModel);

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

            return RedirectToAction("StaffProfile", new { id = _context.UserDetails.SingleOrDefault(c => c.UserId == userId).Id });

        }

        [HttpGet]
        public IActionResult Subscriptions(long id)
        {
            var viewModel = new Models.CustomerViewModels.SubscriptionViewModel();

            var customer = new Customer()
            {
                CompanySubscriptions = _context.Subscriptions.Include(c => c.SubscriptionType).Where(c => c.CompanyId == id).ToList(),
                Company = _context.CompanyDetails.SingleOrDefault(c => c.Id == id)
            };

            viewModel.Customer = customer;
            viewModel.Types = SubscriptionType.GetTypes(_context.SubscriptionTypes.Where(x => x.Id > 0).ToList());

            return View(viewModel);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Subscriptions(Models.CustomerViewModels.SubscriptionViewModel model)
        {

            var companyId = model.Customer.Company.Id;
            var subscription = new Subscription()
            {
                SubscriptionTypeId = model.NewSubscription.SubscriptionTypeId,
                StartDate = DateTime.Now,
                EndDate = new DateTime(2070,01,01),
                UnitPrice = model.NewSubscription.UnitPrice,
                Cost = model.NewSubscription.Cost,
                Url = model.NewSubscription.Url,
                CompanyId = companyId,
                BillingFrequency = model.NewSubscription.BillingFrequency
            };

            var viewModel = new Models.CustomerViewModels.SubscriptionViewModel();

            try
            {
                _context.Add(subscription);
                _context.SaveChanges();
                StatusMessage = "New subscription added.";
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("An error occured when adding subscription: {0}", ex.Message);
                viewModel.NewSubscription = subscription;
            }

            var customer = new Customer()
            {
                CompanySubscriptions = _context.Subscriptions.Include(c => c.SubscriptionType).Where(c => c.CompanyId == companyId).ToList(),
                Company = _context.CompanyDetails.SingleOrDefault(c => c.Id == companyId)
            };

            viewModel.Customer = customer;
            viewModel.StatusMessage = StatusMessage;

            return View(viewModel);
        
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

        #region private methods
        [NonAction]
        private SelectedCustomerViewModel GetSelectedCustomer(long id, bool includeDeleted = false)
        {
            var viewModel = new SelectedCustomerViewModel() { StatusMessage = StatusMessage, includeDeleted = !includeDeleted };
            var company = _context.CompanyDetails.SingleOrDefault(c => c.Id == id);
            List<UserDetail> companyUsers;

            if (includeDeleted)
            {
                companyUsers = _context.UserDetails.Where(x => x.CompanyId == id).Include(c => c.UserContact).Include(p => p.Profile).ToList();
            }
            else
            {
                companyUsers = _context.UserDetails.Where(x => x.CompanyId == id).Where(x => x.EndDate > DateTime.Now).Include(c => c.UserContact).Include(p => p.Profile).ToList();
            }
            int contactIndex = -1;
            int inc = -1;

            var customer = new Customer()
            {
                Company = company,
                Staff = companyUsers
            };

            foreach (var user in companyUsers)
            {
                inc++;
                if (user.UserId == company.UserId)
                {
                    contactIndex = inc;
                }
            }

            if (contactIndex >= 0)
            {
                customer.Contact = companyUsers[contactIndex];
            }
            else
            {
                customer.Contact = new UserDetail() { Name = "Unknown" };
                customer.Contact.UserContact = new UserContact();
                customer.Contact.Profile = new Profile() { Id = 0, Role = "Staff" };
            }

            viewModel.Customer = customer;

            return viewModel;
        }
        [NonAction]
        private StaffMemberViewModel GetStaffMember(long id)
        {
            var model = new StaffMemberViewModel() {StatusMessage = StatusMessage};

            model.Titles = Title.GetTitles(_context.Titles.Where(c => c.Id > 0).OrderBy(x => x.Value).ToList());

            model.UserDetails = _context.UserDetails.Include(x => x.Company).Include(x => x.Profile).Include(x => x.UserContact).SingleOrDefault(x => x.Id == id);
            model.Customer = new Customer() { Company = model.UserDetails.Company};

            if (model.UserDetails.Company.UserId == model.UserDetails.UserId)
            {
                model.IsCompanyContact = true;

                model.Customer.Company.UserId = model.UserDetails.UserId;
            }

            model.UserRoles = GetStaffRoles();
            model.Companies = GetCompanies(model.UserDetails.CompanyId);

            return (model);
        }
        [NonAction]
        private List<RoleDetail> GetUserRoles(string userId)
        {
            var userDetail = _context.UserDetails.SingleOrDefault(x => x.UserId == userId);
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

                if (newRole.TypeId == 0 && userDetail.CompanyId == 1)
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
        [NonAction]
        private List<SelectListItem> GetStaffRoles()
        {
            var outputList = new List<SelectListItem>();

            var resultList = _context.Profiles.ToList();

            foreach (var item in resultList)
            {
                outputList.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Role });
            }

            return outputList;
        }
        [NonAction]
        private CompanyDetail GetCompanyDetails(string userId)
        {
            if (userId == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }

            return _context.CompanyDetails.SingleOrDefault(x => x.UserId == userId);
        }
        [NonAction]
        private List<SelectListItem> GetCompanies(long id)
        {
            List<SelectListItem> outputList = new List<SelectListItem>();
            var resultList = _context.CompanyDetails.ToList();

            foreach (var item in resultList)
            {
                outputList.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
            }

            return outputList;
        }
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

                if (newRole.TypeId == 0)
                {
                    continue;
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
