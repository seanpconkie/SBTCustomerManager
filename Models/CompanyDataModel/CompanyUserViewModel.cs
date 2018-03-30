using System;
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
        [Display(Name = "Company Contact")]
        public bool IsCompanyContact { get; set; }
        #endregion

        #region Methods
        public void SetCompanyContact ()
        {
            IsCompanyContact = true;

            CompanyDetails.UserId = UserDetails.UserId;

        }
        #endregion
    }
}
