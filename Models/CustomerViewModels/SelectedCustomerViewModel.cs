using System;
namespace SBTCustomerManager.Models.CustomerViewModels
{
    public class SelectedCustomerViewModel
    {
        public Customer Customer { get; set; }
        public bool includeDeleted { get; set; }
        public string StatusMessage { get; set; }
    }
}
