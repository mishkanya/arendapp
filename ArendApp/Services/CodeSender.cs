using ArendApp.Models;
using Microsoft.EntityFrameworkCore;

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
            var code = Guid.NewGuid();
            var sendedCode = new SendedCode() { Code = code.ToString(), UserId = user.Id };

            await SendEmail(user.Email, code);

            await _applicationDbContext.SendedCodes.AddAsync(sendedCode);
            await _applicationDbContext.SaveChangesAsync();
            return sendedCode;
        }

        private async Task SendEmail(string email, Guid code)
        {
            await Task.Run(() => { });
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
