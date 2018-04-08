using System;
using System.ComponentModel.DataAnnotations;
namespace SBTCustomerManager.Models.RoleViewModels
{
    public class RoleDetail
    {
        #region Properties
        public string RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Role Type")]
        public int TypeId { get; set; }
        public RoleType Type { get; set; }
        public bool IsSelected { get; set; }
        #endregion
    }
}
