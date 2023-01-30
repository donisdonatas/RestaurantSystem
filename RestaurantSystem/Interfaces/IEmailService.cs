using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSystem.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string mailContext, MailAddress recipientsEmail);
    }
}
