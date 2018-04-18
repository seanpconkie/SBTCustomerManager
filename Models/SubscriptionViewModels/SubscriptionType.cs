using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace SBTCustomerManager.Models.SubscriptionViewModels
{
    public class SubscriptionType
    {
        public int Id { get; set; }
        public string Type { get; set; }

        #region Public Methods
        public static List<SelectListItem> GetTypes(List<SubscriptionType> resultList)
        {

            List<SelectListItem> types = new List<SelectListItem>();

            foreach (var item in resultList)
            {
                types.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Type });
            }

            return types;
        }
        #endregion
    }
}
