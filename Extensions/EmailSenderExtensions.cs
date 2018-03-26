using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using SBTCustomerManager.Services;

namespace SBTCustomerManager.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }

        public static Task SendPasswordChangeNotification(this IEmailSender emailSender, string email)
        {
            return emailSender.SendEmailAsync(email, "Password Changed",
                "Your password has been changed.\nIf this was not you then select 'Forgotten Password' from the login page.");
        }

        public static Task SendPasswordReset(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Reset Password",
                $"Please reset your password by clicking here: <a href='{link}'>link</a>");
        }
    }
}
