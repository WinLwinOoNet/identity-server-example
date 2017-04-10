using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServerWebSite
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme,
                AutomaticAuthenticate = true
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var options = new OpenIdConnectOptions
            {
                SaveTokens = true,
                AuthenticationScheme = OpenIdConnectDefaults.AuthenticationScheme,
                SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme,
                Authority = "http://localhost:5000",
                PostLogoutRedirectUri = "http://localhost:5002",
                RequireHttpsMetadata = false,
                ResponseType = OpenIdConnectResponseType.CodeIdToken,
                ClientId = "testclient",
                ClientSecret = "oursecret"
            };

            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("exampleapi");

            app.UseOpenIdConnectAuthentication(options);

            app.UseMvc();
        }
    }
}
