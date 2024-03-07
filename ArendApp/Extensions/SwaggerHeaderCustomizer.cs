using Microsoft.Build.Evaluation;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ArendApp.Api.Extensions
{
    /// <summary>
    /// Operation filter to add the requirement of the custom header
    /// </summary>
    public class SwaggerHeaderCustomizer : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = HeaderValidatorAttribute.UserTokenHeaderKey,
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            });
        }

    }
}
