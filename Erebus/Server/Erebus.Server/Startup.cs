﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Erebus.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Erebus.Server.Authorization;
using Erebus.Core.Contracts;
using Erebus.Core.Implementations;
using System.Security;
using Erebus.Core.Server.Implementations;
using Erebus.Core.Server.Contracts;
using Erebus.Core.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Erebus.Localization;

namespace Erebus.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var configReader = new ServerConfigurationReader(this.Configuration);

            // Add framework services.
            services.AddMvc(options =>
            {
                //var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                //options.Filters.Add(new AuthorizeFilter(policy));

                if (configReader.GetConfiguration().DisableSSLRequirement == false)
                {
                    options.Filters.Add(new RequireHttpsAttribute());
                }

                options.Filters.Add(typeof(VaultAuthorizationAttribute));

            });

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(configReader.GetConfiguration().SessionTimeoutMinutes);
                options.CookieName = ".Erebus";
            });


            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IServerConfigurationReader>(configReader);
            services.AddTransient<IClockProvider, ClockProvider>();
            services.AddTransient<IFileSystem, ServerFileSystem>();
            services.AddTransient<ISecureStringConverter, SecureStringConverter>();
            services.AddTransient<ISerializer, JsonSerializer>();
            services.AddTransient<ISymetricCryptographer, AesCryptographer>();
            services.AddTransient<IVaultRepositoryFactory, VaultFileRepositoryFactory>();
            services.AddTransient<IVaultManipulatorFactory, VaultManipulatorFactory>();
            services.AddTransient<IVaultFactory, DefaultVaultFactory>();
            services.AddTransient<IPasswordGenerator, PasswordGenerator>();
            services.AddTransient<ISessionContext, SessionContext>();
            services.AddTransient<IAuthorizationLogic, SessionAuthorizationLogic>();
            services.AddTransient<ITimespanFormater, TimespanFormater>();
            services.AddSingleton<ISyncContext, ServerSyncContext>();
            services.AddTransient<IByteArrayHelper, ByteArrayHelper>();
            services.AddTransient<IVaultFileMetadataHandler, VaultFileMetadataHandler>();

            services.AddSingleton<ISecureStringBinarySerializer>(factory =>
            {
                string randomPassword = factory.GetRequiredService<IPasswordGenerator>().GeneratePassword(50, true, true, true, true);
                var secureStringConverter = factory.GetRequiredService<ISecureStringConverter>();
                return new SecureStringBinarySerializer(factory.GetRequiredService<ISymetricCryptographer>(),
                                                        secureStringConverter.ToSecureString(randomPassword),
                                                        factory.GetRequiredService<ISecureStringConverter>());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseSession();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/VaultExplorer/Error");
            }

            app.UseStaticFiles();

            var serverConfiguration = app.ApplicationServices.GetService<IServerConfigurationReader>().GetConfiguration();
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(new CultureInfo(serverConfiguration.Language)),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo(serverConfiguration.Language)
                },
                SupportedUICultures = new List<CultureInfo>
                {
                    new CultureInfo(serverConfiguration.Language)
                }
            });


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=VaultExplorer}/{action=Index}/{id?}");
            });

        }
    }
}
