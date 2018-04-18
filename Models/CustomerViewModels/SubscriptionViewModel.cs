using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using SBTCustomerManager.Models.SubscriptionViewModels;
namespace SBTCustomerManager.Models.CustomerViewModels
{
    public class SubscriptionViewModel
    {
        public Customer Customer { get; set; }
        public Subscription NewSubscription { get; set; }
        public List<SelectListItem> Types { get; set; }
        public List<SelectListItem> Frequencies { get
            {
                var output = new List<SelectListItem>();
                foreach (Frequency freq in Enum.GetValues(typeof(Frequency)))
                {
                    output.Add(new SelectListItem { Value = Convert.ToInt32(freq).ToString(), Text = freq.ToString() });
                }

                return output;

            }
        }
        public string StatusMessage { get; set; }
    }
}
