﻿using System;
using System.ComponentModel.DataAnnotations;
namespace SBTCustomerManager.Models
{
    public class UserContact
    {

        public long Id { get; set; }
        public string UserId { get; set; }

        // Phone
        [Display(Name = "Mobile Phone")]
        public string MobilePhone { get; set; }
        [Display(Name = "Work Phone")]
        public string WorkPhone { get; set; }
        [Display(Name = "Other Phone")]
        public string OtherPhone { get; set; }

        // e-mail

        public string Email { get; set; }

        // Address
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
    }
}
