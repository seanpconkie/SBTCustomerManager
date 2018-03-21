using System;
using SBTCustomerManager.Models;
using SBTCustomerManager.Models.UserDataModels;
using System.Collections.Generic;

namespace SBTCustomerManager.Models.UserDataModels
{
    public class MessageViewModel
    {
        public IEnumerable<UserMessage> UserMessages { get; set; }
        public string StatusMessage { get; set; }
    }
}
