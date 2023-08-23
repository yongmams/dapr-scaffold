using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DarpApp.Admin.API.K8S127
{
    public class AuthResponsesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authorizeAttributes = context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>();
            var authorize = authorizeAttributes.Any();

            if (!authorize)
            {
                var actionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
                if (actionDescriptor != null)
                {
                    var controllerDescriptor = actionDescriptor.ControllerTypeInfo;
                    authorizeAttributes = controllerDescriptor.GetCustomAttributes(true).OfType<AuthorizeAttribute>();
                    authorize = authorizeAttributes.Any();
                }
            }

            if (authorize)
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" } },
                            authorizeAttributes.Select(x => x.Policy).ToList()
                        }
                    }
                 };
            }
        }
    }
}
