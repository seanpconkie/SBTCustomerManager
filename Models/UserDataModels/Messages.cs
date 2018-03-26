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

        public static UserMessage PasswordChanged(string userID, string name)
        {
            return new UserMessage
            {
                UserId = userID,
                Title = "Password Changed",
                Message = string.Format(
@"Your password has been changed.
If this was not you then select 'Forgotten Password' from the login page."
                , name),
                MessageDate = DateTime.Now,
                IsMessageRead = false
            };
        }
    }
}
