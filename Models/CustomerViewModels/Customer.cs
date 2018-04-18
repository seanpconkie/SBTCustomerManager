using System;
using System.Collections.Generic;
using SBTCustomerManager.Models.AccountViewModels;
using SBTCustomerManager.Models.CompanyDataModel;
using SBTCustomerManager.Models.UserDataModels;
using SBTCustomerManager.Models.CustomerViewModels;
using SBTCustomerManager.Models.SubscriptionViewModels;

namespace SBTCustomerManager.Models.CustomerViewModels
{
    public class Customer
    {
        public CompanyDetail Company { get; set; }   
        public UserDetail Contact { get; set; }  
        public List<UserDetail> Staff { get; set; }
        public List<Subscription> CompanySubscriptions { get; set; } 
    }
}
