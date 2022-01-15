using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Alerting.Presentation.Swagger
{
    public static class Utility
    {
        public static void AddCustomizedSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            var swagger = configuration.Get<SwaggerOptions>("Swagger");

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(swagger.Doc.Version, new OpenApiInfo {
                    Title = swagger.Doc.Title,
                    Description = swagger.Doc.Description,
                    Version = swagger.Doc.Version,
                    Contact = new OpenApiContact {
                        Email = swagger.Doc.Contact.Email,
                        Name = swagger.Doc.Contact.Name,
                        Url = new Uri(swagger.Doc.Contact.Url)
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(swagger.OAuth.AuthorizationUrl),
                            TokenUrl = new Uri(swagger.OAuth.TokenUrl),
                            RefreshUrl = new Uri(swagger.OAuth.RefreshUrl),
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "User identifier" }
                            }
                        }
                    }
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
                options.CustomSchemaIds(o => o.FullName);
            });
        }

        public static void UseSwaggerAndSwaggerUI(this IApplicationBuilder app, IConfiguration configuration)
        {
            var swagger = configuration.Get<SwaggerOptions>("Swagger");

            app.UseSwagger();

            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint($"/swagger/{swagger.Doc.Version}/swagger.json", swagger.Doc.Title);
                options.RoutePrefix = "swagger";
                options.DocExpansion(DocExpansion.None);

                options.OAuthClientId("local_api_swagger");
                options.OAuthUsePkce();
            });
        }
    }
}
