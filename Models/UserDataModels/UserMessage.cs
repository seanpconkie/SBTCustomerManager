using System;
namespace SBTCustomerManager.Models.UserDataModels
{
    public class UserMessage
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsMessageRead { get; set; }
        public DateTime? ReadDate { get; set; }
        public DateTime? MessageDate { get; set; }
    }
}
