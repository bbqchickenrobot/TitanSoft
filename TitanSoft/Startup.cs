using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Identity;
using TitanSoft.Api.Middleware;
using TitanSoft.DataAccess;
using TitanSoft.Entities;
using TitanSoft.Helpers;
using TitanSoft.Services;

namespace TitanSoft
{
    public class Startup
    {
        ILoggerFactory loggerFactory;
        public Startup(IConfiguration configuration, ILoggerFactory factory)
        {
            Configuration = configuration;
            loggerFactory = factory;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<ILogger>((sp) => loggerFactory.CreateLogger("general"));
            services.AddRavenDbAsyncSession(RavenDocumentStore.Store)
                    .AddRavenDbIdentity<AppUser>();

            
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.UTF8.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true
                };
            });

            // configure DI for application services
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOmdbApi, OmdbApi>();
            services.AddSingleton<IDocumentStore>(RavenDocumentStore.Store);
            //services.AddScoped<IAsyncDocumentSession>((sp) =>
            //{
            //    return RavenDocumentStore.Store.OpenAsyncSession();
            //});
            services.AddScoped<UserStore<AppUser>>((sp) =>
            {
                return new UserStore<AppUser>(sp.GetService<IAsyncDocumentSession>());
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseRequestLogging(loggerFactory.CreateLogger<PerfLoggingMiddleware>());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseMvc();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();
            
        }
    }
}
