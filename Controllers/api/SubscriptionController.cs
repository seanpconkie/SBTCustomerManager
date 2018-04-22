using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SBTCustomerManager.Data;
using SBTCustomerManager.Services;
using SBTCustomerManager.Models.UserDataModels;
using SBTCustomerManager.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SBTCustomerManager.Controllers.api
{
    [Route("api/[controller]")]
    public class SubscriptionController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public SubscriptionController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<MessagesController> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var subscription = _context.Subscriptions.SingleOrDefault(c => c.Id == id);

            return new ObjectResult(subscription);
        }
    }
}
