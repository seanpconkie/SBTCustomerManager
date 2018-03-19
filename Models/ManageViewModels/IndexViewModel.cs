using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SBTCustomerManager.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }

        public UserDetail UserDetail { get; set; }
        public string Title
        {
            get
            {
                if (UserDetail.Name == null)
                    return "Edit Profile";

                return "Edit" + UserDetail.Name + "'s Profile";
            }
        }


    }
}
