using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace AngularAdo.Web.App.Extensions
{
    public static class StartupExtension
    {
        /// <summary>
        /// Add MVC configuration and endpoint policies
        /// </summary>
        public static IServiceCollection AddMvcBehavior(this IServiceCollection services)
        {
            _ = services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });

            _ = services.AddHsts(options =>
            {
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromMilliseconds(31536000000);
            });

            return services;
        }

        /// <summary>
        /// Register  ApiVersioning
        /// </summary>
        public static IServiceCollection ConfigureApiVersioning(this IServiceCollection services)
        {
            object p = services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });
            return services;
        }

        public static IApplicationBuilder ConfigureCors(this IApplicationBuilder app, IConfiguration configuration)
        {
            var allowedOrigins = configuration.GetValue("CORS:Origins", "*");

            _ = app.UseHttpsRedirection();

            _ = app.UseCors(builder =>
            {
                if (allowedOrigins == "*")
                {
                    _ = builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                }
                else
                {
                    _ = builder.SetIsOriginAllowedToAllowWildcardSubdomains()
                        .WithOrigins(allowedOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries))
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                }
            });

            return app;
        }

        public static IApplicationBuilder ConfigureHeaders(this IApplicationBuilder app, string nonce)
        {
            // REQUIRED by infosec
            _ = app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue
                    {
                        NoCache = true,
                        NoStore = true,
                        MustRevalidate = true
                    };

                // Prohibited Headers
                _ = context.Response.Headers.Remove("splitsdkversion");
                _ = context.Response.Headers.Remove("x-aspnet-version");
                _ = context.Response.Headers.Remove("x-powered-by");
                _ = context.Response.Headers.Remove("server");

                // Required Headers
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("X-Xss-Protection", "1;mode=block");
                context.Response.Headers.Add("Content-Security-Policy", $"default-src 'self';img-src data: https:;object-src 'nonce-{nonce}'; script-src https://stackpath.bootstrapcdn.com/ 'self' 'nonce-{nonce}';style-src https://stackpath.bootstrapcdn.com/ 'self' 'nonce'; upgrade-insecure-requests;");

                await next().ConfigureAwait(false);
            });

            return app;
        }
    }
}