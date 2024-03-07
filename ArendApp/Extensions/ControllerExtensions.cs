using ArendApp.Api.Services;
using ArendApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArendApp.Api.Extensions
{
    public static class ControllerExtensions
    {
        public static string GetServerDomen(this ControllerBase controller) => $"{controller.Request.Scheme}://{controller.Request.Host}{controller.Request.PathBase}/";

        public static async Task<User> GetUserAsync(this ControllerBase controller)
        {
            var db = controller.HttpContext.RequestServices.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            if(controller.Request.Headers.TryGetValue(HeaderValidatorAttribute.UserTokenHeaderKey, out var userToken) == false)
            {
                return null ;
            }
            var user = await db.UsersData.FirstOrDefaultAsync(u => u.Token == userToken.ToString());
            return user;
        }
    }
}
