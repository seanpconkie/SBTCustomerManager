using System;
using System.ComponentModel.DataAnnotations;

namespace SBTCustomerManager.Models
{
    public class CompanyDetails
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string UserId { get; set; }

    }
}
