using System;
using System.ComponentModel.DataAnnotations;
using SBTCustomerManager.Models.CompanyDataModel;

namespace SBTCustomerManager.Models.UserDataModels
{
    public class UserDetail
    {

        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string ForeName { get; set; }
        [Required]
        [StringLength(100)]
        public string Surname { get; set; }
        [Required]
        public string Title { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        public long Id { get; set; }
        public string UserId { get; set; }

        public UserContact UserContact { get; set; }

        [Display(Name = "Contact Details")]
        public long UserContactId { get; set; }

        public CompanyDetail Company { get; set; }

        [Display(Name = "Company Details")]
        public long CompanyId { get; set; }

        [Display(Name = "Position")]
        public long ProfileId { get; set; }
        public Profile Profile { get; set; }

    }
}
