using System;
using System.ComponentModel.DataAnnotations;
namespace SBTCustomerManager.Models.CompanyDataModel
{
    public class Subscription
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }

        [Display(Name = "Subscription Type")]
        public int SubscriptionTypeId { get; set; }
        public SubscriptionType SubscriptionType { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public decimal Cost { get; set; }

        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Billing Frequency")]
        public Frequency BillingFrequency { get; set; }

        public string Url { get; set; }

        public int Users { get; set; }

    }
}
