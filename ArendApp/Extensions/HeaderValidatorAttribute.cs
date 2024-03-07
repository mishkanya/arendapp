using ArendApp.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArendApp.Api.Extensions
{
    public class HeaderValidatorAttribute : Attribute, IResourceFilter
    {
        private readonly bool _adminOnly;
        public const string UserTokenHeaderKey = "UserToken";

        public HeaderValidatorAttribute(bool adminOnly)
        {
            _adminOnly = adminOnly;
        }
        public HeaderValidatorAttribute()
        {
            _adminOnly = false;
        }


        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var dbContext = context.HttpContext
            .RequestServices
            .GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

            bool tokenIsValid = context.HttpContext.Request.Headers.TryGetValue(UserTokenHeaderKey, out var token);
            if (tokenIsValid)
            {
                tokenIsValid = string.IsNullOrWhiteSpace(token) == false;
            }
            if (tokenIsValid == false)
            {
                context.Result = new ForbidResult("No token for request");
            }

            if (_adminOnly)
                if (dbContext.UsersData.FirstOrDefault(t => t.Token.ToLower() == token.ToString().ToLower()) == null)
                    context.Result = new ForbidResult("User is not admin");
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {

        }
    }
}