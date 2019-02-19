using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreAPI1.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace NetCoreAPI1
{
   /// <summary>
   /// startup class 
   /// </summary>
   public class Startup
   {
      /// <inheritdoc />
      public Startup(IConfiguration configuration)
      {
         Configuration = configuration;
      }

      /// <summary>
      /// configuration service
      /// </summary>
      public IConfiguration Configuration { get; }


      /// <summary>
      /// This method gets called by the runtime. Use this method to add services to the container.
      /// </summary>
      /// <param name="services"></param>
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddDbContext<TodoContext>(opt =>
             opt.UseInMemoryDatabase("TodoList"));
         services.AddSwaggerGen(c =>
         {
            c.SwaggerDoc("v1", new Info
            {
               Version = "v1",
               Title = "ToDo API",
               Description = "A simple example ASP.NET Core Web API",
               TermsOfService = "None",
               Contact = new Contact
               {
                  Name = "Shayne Boyer",
                  Email = string.Empty,
                  Url = "https://twitter.com/spboyer"
               },
               License = new License
               {
                  Name = "Use under LICX",
                  Url = "https://example.com/license"
               }
            });

            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
         });

         services.AddMvc(opt =>
         {
            opt.OutputFormatters.RemoveType<TextOutputFormatter>(); // elimina soporte para respuesta en formato texto
            opt.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>(); // elimina soporte para nulos
         })
         .AddXmlSerializerFormatters() //agrega soporte para formato XML
         .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
      }


      /// <summary>
      /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      /// </summary>
      /// <param name="app"></param>
      /// <param name="env"></param>
      public void Configure(IApplicationBuilder app, IHostingEnvironment env)
      {

         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }
         else
         {
            app.UseHsts();
         }

         // enable swagger
         app.UseSwagger();
         // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
         // specifying the Swagger JSON endpoint.
         app.UseSwaggerUI(c =>
         {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            c.RoutePrefix = string.Empty;
         });

         app.UseHttpsRedirection();
         app.UseMvc();
      }
   }
}
