﻿using System;
using System.Text;
using CacheManager.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Raven.Client.Documents.Session;
using Raven.Identity;
using Swashbuckle.AspNetCore.Swagger;
using TitanSoft.Api.Middleware;
using TitanSoft.Api.Services;
using TitanSoft.DataAccess;
using TitanSoft.Entities;
using TitanSoft.Helpers;
using TitanSoft.Services;
using CacheManager.Core.Utility;
using CacheManager.MicrosoftCachingMemory;
using Microsoft.AspNetCore.Identity;

namespace TitanSoft
{
    public class Startup
    {
        ILoggerFactory loggerFactory;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, ILoggerFactory factory)
        {
            Configuration = configuration;
            loggerFactory = factory;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton((sp) => loggerFactory.CreateLogger("general"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "TitanSoft Movies API", Version = "v1" });
            });

            services.AddRavenDbAsyncSession(RavenDocumentStore.Store)
                .AddRavenDbIdentity<AppUser>(options =>
                {
                        // Password settings.
                        options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;

                        // Lockout settings.
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 15;
                options.Lockout.AllowedForNewUsers = true;

                        // User settings.
                        options.User.AllowedUserNameCharacters =
                            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
                });

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.UTF8.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
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

            services.AddAuthorization((o) =>
            {

            });


            services.AddSingleton(Configuration);
            services.AddScoped<IUserService, MemberService>();
            services.AddScoped<IOmdbApi, OmdbApi>();
            services.AddSingleton(RavenDocumentStore.Store);
            services.AddScoped((sp) => new UserStore<AppUser>(sp.GetService<IAsyncDocumentSession>()));
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IRentalService, RentalService>();
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
                app.UseExceptionHandlingMiddleware();
            }

            app.UseHttpsRedirection();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TitanSoft Movie API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
