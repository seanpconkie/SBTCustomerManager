using System;
using System.ComponentModel.DataAnnotations;

namespace SBTCustomerManager.Models.CompanyDataModel
{
    public class CompanyDetail
    {
        public long Id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Company Name")]
        public string Name { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string UserId { get; set; }

    }
}
