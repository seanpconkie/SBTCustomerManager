using System;
using System.Collections.Generic;
using SBTCustomerManager.Models.CompanyDataModel;
using SBTCustomerManager.Models.UserDataModels;
namespace SBTCustomerManager.Models.CustomerViewModels
{
    public class CustomerViewModel
    {
        public List<Customer> Customers { get; set; }
        public string StatusMessage { get; set; }
    }
}
