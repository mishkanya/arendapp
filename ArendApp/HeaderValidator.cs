using ArendApp.Api.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArendApp.Api
{
    public class HeaderValidator : Attribute, IResourceFilter
    {
        private readonly bool _adminOnly;
        private const string _userTokenHeatderKey = "UserToken";

        public HeaderValidator(bool adminOnly)
        {
            _adminOnly = adminOnly;
        }
        public HeaderValidator()
        {
            _adminOnly = false;
        }


        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var dbContext = context.HttpContext
           .RequestServices
           .GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

            bool tokenIsValid = context.HttpContext.Request.Headers.TryGetValue(_userTokenHeatderKey, out var token);
            if (tokenIsValid)
            {
                tokenIsValid = string.IsNullOrWhiteSpace(token) == false;
            }
            if(tokenIsValid == false)
            {
                throw new Exception("No token for request");
            }

            if(_adminOnly)
                if(dbContext.UsersData.FirstOrDefault(t => t.Token.Equals(token, StringComparison.OrdinalIgnoreCase)) == null)
                    throw new Exception("User is not admin");
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {

        }
    }
}