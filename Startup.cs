using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SkopeiBackendAssignment.DataContext;

using SkopeiBackendAssignment.Entities;
using SkopeiBackendAssignment.DataManager;
using SkopeiBackendAssignment.Entities.IRepository;
using System.Reflection;
using System.IO;
using Microsoft.OpenApi.Models;

namespace SkopeiBackendAssignment
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
            // DB Context Pointing to SQL Server Database based on connection string in appsettings.json **/
            services.AddDbContext<TestDataContext>(opt =>
            opt.UseSqlServer(Configuration["ConnectionString:TestDB"]));
            // The following lines are used for DI (Dependency Injection) - Using this separates our API Controller level from the data layer**/
            services.AddScoped<IDataManager<User>, UserDataManager>();
            services.AddScoped<IDataManager<Product>, ProductDataManager>();

            // Adding controllers to project **/
            services.AddControllers();

            // Swagger Documentation
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Skopei Test API",
                    Version = "1.0.0",
                    Description = "API Documentation for Skopei Backend Assignment",
                    Contact = new OpenApiContact
                    {
                        Name = "Debadeep Basu",
                        Email = "debadeep.basu@gmail.com"
                    },
                });

                // Setting the comments path for the Swagger JSON and UI.
                // The xml file generated due to enabling XML Comments is included here
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                s.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // To Serve Swagger files for APIs as JSON endpoint
            app.UseSwagger();
            // To Serve  Swagger UI for the APIs using swagger json endpoint
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                // To load Swagger by default route
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



        }
    }
}
