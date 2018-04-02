using System;
using System.ComponentModel.DataAnnotations;
namespace SBTCustomerManager.Models.UserDataModels
{
    public class RoleTypes
    {
        public int Id { get; set; }
        [Key]
        public string RoleId { get; set; }
    }
}
