using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace MWALIMU_CLASSIC_LMS.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // For now, we'll just return a completed task since you may not need email functionality yet
            // You can implement actual email sending logic here later
            return Task.CompletedTask;
        }
    }
}