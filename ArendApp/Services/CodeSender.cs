using ArendApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

namespace ArendApp.Api.Services
{
    public class CodeSender
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CodeSender(ApplicationDbContext applicationDbContext) 
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<SendedCode> SendCode(User user)
        {
            var code = new Random().Next(100000,999999); // Guid.NewGuid();
            var sendedCode = new SendedCode() { Code = code.ToString(), UserId = user.Id };


            await _applicationDbContext.SendedCodes.AddAsync(sendedCode);
            await _applicationDbContext.SaveChangesAsync();
            await SendEmail(sendedCode, user.Email);
            return sendedCode;
        }

        private async Task SendEmail(SendedCode code, string email)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("confirmcode@mail.ru");
            mailMessage.To.Add(email);
            mailMessage.Subject = "Код подтверждения";
            mailMessage.Body = $"Код подтверждения учетной записи {code.Code}\n" +
                $"Время действия до {code.Limit}";

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.mail.ru";
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("confirmcode@mail.ru", "6TXZskMxi4jDyCMCyp8G");
            smtpClient.EnableSsl = true;

            smtpClient.Send(mailMessage);
        }

        

        public async Task<bool> CodeVerification(int userId, string code)
        {
            var sendedCode = await _applicationDbContext.SendedCodes.FirstOrDefaultAsync(t => t.UserId == userId && t.Code.Equals(code, StringComparison.OrdinalIgnoreCase) && t.Limit < DateTime.Now);
            if(sendedCode != null)
            {
                var userData = await _applicationDbContext.UsersData.FindAsync(userId);
                userData.Confirmed = true;
                _applicationDbContext.SendedCodes.Remove(sendedCode);

                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
