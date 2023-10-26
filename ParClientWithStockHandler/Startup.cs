using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.Extensions.Configuration;

namespace ParClientWithStockHandler
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ParEvents>();

            // add MVC
            services.AddControllersWithViews();

            // add cookie-based session management with OpenID Connect authentication
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "cookie";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("cookie", options =>
                {
                    options.Cookie.Name = "par-default-handler";

                    options.ExpireTimeSpan = TimeSpan.FromHours(8);
                    options.SlidingExpiration = false;
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "par-default-handler";
                    options.ClientSecret = "secret";

                    options.ResponseType = "code";
                    options.UsePkce = true;

                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("offline_access");

                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;
                    options.MapInboundClaims = false;
                    
                    options.EventsType = typeof(ParEvents);
                    
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };

                    options.DisableTelemetry = true;
                });

            // add automatic token management
            services.AddOpenIdConnectAccessTokenManagement();

            // add HTTP client to call protected API
            services.AddUserAccessTokenHttpClient("client", configureClient: client =>
            {
                client.BaseAddress = new Uri("https://localhost:7001");
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute()
                    .RequireAuthorization();
            });
        }
    }
}