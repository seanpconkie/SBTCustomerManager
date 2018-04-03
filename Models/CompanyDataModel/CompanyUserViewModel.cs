using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using SBTCustomerManager.Models.CompanyDataModel;
using SBTCustomerManager.Models.UserDataModels;

namespace SBTCustomerManager.Models.CompanyDataModel
{
    public class CompanyUserViewModel
    {
        #region Properties
        public UserDetail UserDetails { get; set; }
        public CompanyDetail CompanyDetails { get; set; }
        public string StatusMessage { get; set; }
        public List<SelectListItem> UserRoles { get; set; }
        [Display(Name = "Company Contact")]
        public bool IsCompanyContact { get; set; }
        public List<RoleDetail> RoleList { get; set; }
        #endregion

    }
}
