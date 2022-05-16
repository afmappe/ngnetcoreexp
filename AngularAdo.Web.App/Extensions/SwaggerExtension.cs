﻿using Microsoft.OpenApi.Models;
using System.Text;

namespace AngularAdo.Web.App.Extensions
{
    public static class SwaggerExtension
    {
        private const string ApiBaseAssambly = "AngularAdo.Web.App";
        private const string ApiContactMail = "";
        private const string ApiDescription = "ASP.NET Core Web API";
        private const string ApiName = "Example API";
        private const string ApiReleaseNotes = "https://localhost:7231/swagger";
        private const string ApiRouteTemplate = "docs/{documentName}/docs.json";
        private const string ApiTeamName = "";
        private const string ApiTitle = "Example API";
        private const string ApiUrl = "/docs/v1/docs.json";
        private const string ApiVersion = "v1";

        /// <summary>
        /// Add swagger to API
        /// </summary>
        public static IServiceCollection ConfigureApiDocumentation(this IServiceCollection services)
        {
            var releaseNotes =

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(e => e.FullName);
                c.CustomOperationIds(e => $"{e.HttpMethod}-{e.ActionDescriptor.RouteValues["action"]}-{e.GroupName}");
                c.SwaggerDoc(ApiVersion, GetOpenApiInfo(ApiTitle, ApiVersion, ApiDescription, new Uri(ApiReleaseNotes)));

                var xmlFiles = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Where(assembly => assembly.FullName.Contains(ApiBaseAssambly))
                    .Select(assembly => Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? "BaseDirectoryNotFound",
                        $"{assembly.GetName().Name}.xml"));

                foreach (var xmlFile in xmlFiles)
                {
                    if (File.Exists(xmlFile))
                    {
                        c.IncludeXmlComments(xmlFile);
                    }
                }

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization Header {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                  });
            });

            return services;
        }        /// <summary>

                 /// Configure Swagger
                 /// </summary>
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, string nonce)
        {
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = false;
                c.RouteTemplate = ApiRouteTemplate;
            });

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "docs";
                c.SwaggerEndpoint(ApiUrl, ApiName);

                // 2. Take a reference of the original Stream factory which reads from Swashbuckle's embedded resources
                var originalIndexStreamFactory = c.IndexStream;

                // 3. Override the Stream factory
                c.IndexStream = () =>
                {
                    // 4. Read the original index.html file
                    using var originalStream = originalIndexStreamFactory();
                    using var originalStreamReader = new StreamReader(originalStream);
                    var originalIndexHtmlContents = originalStreamReader.ReadToEnd();

                    // 5. Get the request-specific nonce generated by NetEscapades.AspNetCore.SecurityHeaders
                    var requestSpecificNonce = nonce;

                    // 6. Replace inline `<script>` and `<style>` tags by adding a `nonce` attribute to them
                    var nonceEnabledIndexHtmlContents = originalIndexHtmlContents
                        .Replace("<script>", $"<script nonce=\"{requestSpecificNonce}\">", StringComparison.OrdinalIgnoreCase)
                        .Replace("<style>", $"<style nonce=\"{requestSpecificNonce}\">", StringComparison.OrdinalIgnoreCase);

                    // 7. Return a new Stream that contains our modified contents
                    return new MemoryStream(Encoding.UTF8.GetBytes(nonceEnabledIndexHtmlContents));
                };
            });

            return app;
        }

        /// <summary>
        /// Get OpenApiInfo
        /// </summary>
        private static OpenApiInfo GetOpenApiInfo(string title, string version, string description, Uri releaseNotes)
        {
            var oai = new OpenApiInfo
            {
                Title = title,
                Version = version,
                Contact = new OpenApiContact { Email = ApiContactMail, Name = ApiTeamName },
                Description = description
            };
            if (releaseNotes != null) oai.Contact.Url = releaseNotes;
            return oai;
        }
    }
}