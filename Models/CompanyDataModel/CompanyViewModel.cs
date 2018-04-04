using System;
using SBTCustomerManager.Models.UserDataModels;
using SBTCustomerManager.Models.SubscriptionViewModels;
using System.Collections.Generic;
namespace SBTCustomerManager.Models.CompanyDataModel
{
    public class CompanyViewModel
    {
        public UserDetail CompanyContact { get; set; }
        public CompanyDetail CompanyDetails { get; set; }
        public IEnumerable<UserDetail> CompanyUsers { get; set; }
        public IEnumerable<Subscription> CompanySubscriptions { get; set; }
        public string StatusMessage { get; set; }
    }
}
