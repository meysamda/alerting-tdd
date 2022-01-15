using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Alerting.Presentation.Swagger
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAuthorize = context.ApiDescription.ActionDescriptor.EndpointMetadata.Any(o => o is AuthorizeAttribute);
            var hasAnonymous = context.ApiDescription.ActionDescriptor.EndpointMetadata.Any(o => o is AllowAnonymousAttribute);

            if (hasAuthorize && !hasAnonymous)
            {
                var alreadyAdded = operation.Responses.Any(o => o.Key == "401");
                if (!alreadyAdded) operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });

                alreadyAdded = operation.Responses.Any(o => o.Key == "403");
                if (!alreadyAdded) operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                operation.Security = new List<OpenApiSecurityRequirement>
                    {
                        new OpenApiSecurityRequirement {
                            [
                                new OpenApiSecurityScheme {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "oauth2"
                                    },
                                }
                            ] = new[] { "openid" }
                        }
                    };
            }
        }
    }
}
