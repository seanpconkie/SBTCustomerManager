using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SBTCustomerManager.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        #region Properties
        // Asp.net identity properties
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        // App User details
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [Display(Name = "Fore name")]
        public string ForeName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required]
        public string Title { get; set; }
        public IEnumerable<SelectListItem> Titles { get; set; }

        // App user contact
        [Phone]
        [Display(Name = "Mobile Phone")]
        public string MobilePhone { get; set; }
        [Phone]
        [Display(Name = "Work Phone")]
        public string WorkPhone { get; set; }
        [Phone]
        [Display(Name = "Other Phone")]
        public string OtherPhone { get; set; }

        [Display(Name = "Building Number")]
        public string BuildingNumber { get; set; }
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }
        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }
        [Display(Name = "Post Town")]
        public string PostTown { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }

        //Company
        [StringLength(100)]
        [Display(Name = "Company name")]
        public string CompanyName { get; set; }
        #endregion

        #region Constructors
        public RegisterViewModel()
        {
            //Titles = this.GetTitles();
        }
        #endregion
        #region Private Methods
        private IEnumerable<SelectListItem> GetTitles()
        {
            return new SelectListItem[]
            {
            new SelectListItem() { Text = "Dr", Value = "Dr" },
            new SelectListItem() { Text = "Miss", Value = "Miss" },
            new SelectListItem() { Text = "Mr", Value = "Mr" },
            new SelectListItem() { Text = "Mrs", Value = "Mrs" },
            new SelectListItem() { Text = "Ms", Value = "Ms" },
            new SelectListItem() { Text = "Other", Value = string.Empty }
            };
        }
        #endregion


    }

   

}
