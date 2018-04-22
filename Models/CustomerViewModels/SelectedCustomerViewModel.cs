using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using SBTCustomerManager.Models.CompanyDataModel;
using SBTCustomerManager.Models.UserDataModels;
using SBTCustomerManager.Models.RoleViewModels;

namespace SBTCustomerManager.Models.CustomerViewModels
{
    public class SelectedCustomerViewModel
    {
        public Customer Customer { get; set; }
        public string Address { get; set; }
        public bool includeDeleted { get; set; }
        public string StatusMessage { get; set; }

        #region Select Lists
        public List<SelectListItem> Subscriptions { get; set; }
        #endregion
    }
}
