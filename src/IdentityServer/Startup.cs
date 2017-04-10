using System.Collections.Generic;
using IdentityServer.Context;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ExampleDbContext>(options =>
            {
                options.UseInMemoryDatabase();
            });
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedEmail = false;
                    /*options.Cookies.ApplicationCookie.LoginPath = "/account/login";
                    options.Cookies.ApplicationCookie.LogoutPath = "/account/logout";*/
                }
            ).AddEntityFrameworkStores<ExampleDbContext>();

            services.AddIdentityServer()
                .AddTemporarySigningCredential()
                .AddAspNetIdentity<ApplicationUser>()
                .AddInMemoryIdentityResources(new List<IdentityResource>
                {
                    new IdentityResources.OpenId()
                })
                .AddInMemoryApiResources(new List<ApiResource>
                {
                    new ApiResource("exampleapi")
                })
                .AddInMemoryClients(new List<Client>
                {
                    new Client
                    {
                        ClientId = "testclient",
                        RequireConsent = false,
                        RedirectUris = {"http://localhost:5002/signin-oidc"},
                        PostLogoutRedirectUris = {"http://localhost:5002/signout-callback-oidc"},
                        AllowedGrantTypes = new List<string> {GrantType.Hybrid},
                        ClientSecrets = {new Secret("oursecret".Sha256())},
                        AllowedScopes = {"exampleapi", "openid"}
                    }
                });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var userManager = app.ApplicationServices.GetService<UserManager<ApplicationUser>>();

            var createTaks = userManager.CreateAsync(new ApplicationUser
            {
                Email = "test@email.com",
                UserName = "test@email.com",
            }, "P@ssw0rd1234");
            createTaks.Wait();

            app.UseIdentity();
            app.UseIdentityServer();

            app.UseMvcWithDefaultRoute();
        }
    }
}