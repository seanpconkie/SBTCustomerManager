using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace SBTCustomerManager.Models.RoleViewModels
{
    public class RoleViewModel
    {
        public IEnumerable<RoleDetail> Roles { get; set; } 
        public List<SelectListItem> RoleTypes { get; set; }  
        public RoleDetail Role { get; set; }
        public string StatusMessage { get; set; }
    }
}
