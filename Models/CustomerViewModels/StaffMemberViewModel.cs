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
    public class StaffMemberViewModel
    {
        public UserDetail UserDetails { get; set; }
        public List<RoleDetail> RoleList { get; set; }
        public Customer Customer { get; set; }

        [Display(Name = "Company Contact")]
        public bool IsCompanyContact { get; set; }
        public List<SelectListItem> Companies { get; set; }

        #region Select Lists
        public List<SelectListItem> Titles { get; set; } 
        public List<SelectListItem> UserRoles { get; set; }
        #endregion

        public string StatusMessage { get; set; }

    }
}
