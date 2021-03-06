using System;
using DinnerSpinner.Domain.DomainServices;
using DinnerSpinner.Domain.Repositories;
using DinnerSpinner.Infrastructure.MongoDB;
using DinnerSpinner.Web.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace DinnerSpinner.Api
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
            // requires using Microsoft.Extensions.Options
            services.Configure<DatabaseSettings>(
                Configuration.GetSection(nameof(DatabaseSettings)));

            services.AddSingleton<IDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            services.AddScoped<ISpinnerRepository, MongoDbSpinnerRepository>();

            services.AddScoped<SpinnerService>();

            services.AddMongoDbConfiguration();

            services.AddSwaggerDocumentation();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    //builder.AllowAnyOrigin();
                    //builder.AllowAnyHeader();
                    //builder.AllowAnyMethod();

                    builder.AllowAnyMethod().AllowAnyHeader();
                    builder.SetIsOriginAllowed((host) => true);
                    builder.AllowCredentials();
                });
            });
            services.AddControllers();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var runningInContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");

            if (env.IsProduction() && runningInContainer?.ToLower() != "true")
                app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseSwaggerDocumentation();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(config =>
            {
                config.MapControllers();
                config.MapFallbackToFile("index.html").AllowAnonymous();
            });
        }
    }
}