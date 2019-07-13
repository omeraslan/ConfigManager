﻿using ConfigManager.Core.Contracts;
using ConfigManager.Core.DTOs;
using ConfigManager.Core.Enums;
using ConfigManager.Core.Factory;
using ConfigManager.Core.QuartzScheduler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace ConfigManager.WebAPI
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

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSingleton<IConfigurationEngineFactory, ConfigurationEngineFactory>();
            var factory = services.BuildServiceProvider().GetService<IConfigurationEngineFactory>();

            services.AddSingleton<IConfigurationEngine>(s => factory.Create("services",
                new ConnectionDTO("mongodb://localhost:27017/ConfigDb", StorageProviderType.MongoDb), 120000));

            services.AddCacheRefreshQuartzScheduler(120000);

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" }); });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseQuartz();
        }
    }
}
