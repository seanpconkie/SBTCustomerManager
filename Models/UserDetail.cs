using System;
using System.ComponentModel.DataAnnotations;

namespace SBTCustomerManager.Models
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

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int Id { get; set; }
        public string UserId { get; set; }


        public UserContact UserContact { get; set; }

        [Display(Name = "Contact Details")]
        public int UserContactId { get; set; }

    }
}
