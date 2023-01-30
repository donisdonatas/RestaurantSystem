﻿using System.Net.Mail;
using System.Net;
using RestaurantSystem.Interfaces;

namespace RestaurantSystem.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string mailContext, MailAddress recipientsEmail)
        {
            try
            {
                var fromAddress = new MailAddress("donisdonatas@gmail.com", "Donatas");
                var toAddress = recipientsEmail;
                const string fromPassword = "oqdtnnytijhqtowt";
                const string subject = "Restorano sąskaita";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 2000
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = mailContext,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Sąskaita išsiųsta el. paštu.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Iškilo problema bandant išsųsti laišką: {ex}");
                Console.WriteLine("Patikrinkite parametrus ir bandykite dar kartą.");
            }
        }
    }
}
