using System;
namespace SBTCustomerManager.Models.UserDataModels
{
    public static class Messages
    {
        public static UserMessage WelcomeMessage(string userID,string name)
        {
            return new UserMessage{
                UserId = userID,
                Title = "Welcome",
                Message = string.Format("Hi {0}!\nWelcome to SBT Solutions.", name),
                MessageDate = DateTime.Now,
                IsMessageRead = false
            };
        }
    }
}
