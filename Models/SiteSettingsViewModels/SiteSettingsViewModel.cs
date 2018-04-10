using System;
using System.Collections.Generic;
using SBTCustomerManager.Models.RoleViewModels;
using SBTCustomerManager.Models.UserDataModels;
namespace SBTCustomerManager.Models.SiteSettingsViewModels
{
    public class SiteSettingsViewModel
    {
        public List<RoleType> RoleTypes { get; set; }  
        public List<Profile> Profiles { get; set; } 
        public List<Title> Titles { get; set; }  
        public string UpdateType { get; set; }
        public string UpdateValue { get; set; }
        public long UpdateId { get; set; }
        public string StatusMessage { get; set; }
    }
}
