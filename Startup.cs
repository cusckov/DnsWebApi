using DnsWebApi.Services;
using DnsWebApi.Services.DatabaseStrategy;
using DnsWebApi.Services.DatabaseStrategy.Interfaces;
using DnsWebApi.Services.DatabaseStrategy.Strategies;
using DnsWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace DnsWebApi
{
    /// <summary>
    /// Объект создаваемый при запуске приложения
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="configuration">Параметры appsettings.json</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Объект для конфигурирования приложения из appsettings.json
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<IDatabaseStrategy, MSSQLDatabase>();
            services.AddScoped<DatabaseStrategyContext>();

            services.AddControllers();

            services.AddSwaggerGen(swagger =>
            {
                // swagger создается вместе с проектом
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "WebApi App for Manage Notes (Dns's test work)",
                    Description = "Тестовое задание для \"Dns\""
                });
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DnsWebApi v1"));
            }
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
