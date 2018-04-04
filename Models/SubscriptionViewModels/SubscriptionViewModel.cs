using System;
using System.Collections.Generic;
using SBTCustomerManager.Models.SubscriptionViewModels;
namespace SBTCustomerManager.Models.SubscriptionViewModels
{
    public class SubscriptionViewModel
    {
        public IEnumerable<SubscriptionType> Types { get; set; }
        public string StatusMessage { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
    }
}
