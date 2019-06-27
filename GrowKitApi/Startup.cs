using GrowKitApi.Contexts;
using GrowKitApi.Services;
using GrowKitApi.SettingModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;

namespace GrowKitApi
{
    /// <summary> The class holding instructions performed upon startup of the web server.</summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        ///<summary> This method gets called by the runtime. Use this method to add services to the container.</summary>
        public void ConfigureServices(IServiceCollection services)
        {
            // read the settings from the appsettings.json file
            services.Configure<TokenSettings>(Configuration.GetSection("TokenSettings"));
            services.Configure<MailProviderSettings>(Configuration.GetSection("MailProviderSettings"));

            var tokenSettings = Configuration.GetSection("TokenSettings").Get<TokenSettings>();
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Secret));

            // attach services to the service provider so that they can be used for dependency injection.
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<IEmailService, EmailService>();

            // set up the database contexts that will be injected into the program.
            services.AddDbContext<AuthenticationContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("AuthenticationDbString"));
            });
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("ApplicationDbString"));
            });

            // set up the authentication to use Json Web Tokens for validation
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = tokenSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = tokenSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = tokenSettings.Issuer;
                configureOptions.Audience = tokenSettings.Audience;
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .RequireClaim("sub")
                .RequireClaim("authorized", "true").Build();
            });

            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver()).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        ///<summary> This method gets called by the runtime. Use this method to configure the HTTP request pipeline</summary>
        ///<remarks> Https is nor enforced heredue to time constraints with setting up the server.</remarks>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            { 
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
