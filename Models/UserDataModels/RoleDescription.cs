using System;
using System.ComponentModel.DataAnnotations;
namespace SBTCustomerManager.Models.UserDataModels
{
    public class RoleDescription
    {
        [Key]
        public string RoleId { get; set; }
        public string Description { get; set; }
    }
}
