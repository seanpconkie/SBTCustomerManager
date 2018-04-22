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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SBTCustomerManager.Controllers.api
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public MessagesController(
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
        // GET api/Messages/{..}
        [HttpGet("{userId}")]
        public IEnumerable<UserMessage> GetMessages(string userId)
        {
            return _context.UserMessages.Where(c => c.UserId == userId).ToList();
        }

        // GET api/Messages/GetMessages/1
        [HttpPut("{id}")]
        public IActionResult Get(int id)
        {
            var message = _context.UserMessages.SingleOrDefault(c => c.Id == id);

            if (message == null)
                return NotFound();

            message.IsMessageRead = true;
            message.ReadDate = DateTime.Now;

            _context.Update(message);
            _context.SaveChanges();

            return new ObjectResult(message);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]UserMessage userMessage)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _context.Add(userMessage);
            _context.SaveChanges();

            return CreatedAtRoute("CreateMessage", new { id = userMessage.Id });
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            var message = _context.UserMessages.SingleOrDefault(c => c.Id == id);

            if (message == null)
                return BadRequest();

            _context.UserMessages.Remove(message);
            _context.SaveChanges();

            return new NoContentResult();

        }
    }
}
