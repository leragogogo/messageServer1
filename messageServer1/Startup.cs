using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace messageServer1
{
    /// <summary>
    /// ����������� �����.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ����������� ������.
        /// </summary>
        /// <param name="configuration">������������</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// ������������.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ���������� ����� � ���������.
        /// </summary>
        /// <param name="services">�������.</param>
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "messageServer1", Version = "v1" });
                var filePath = Path.Combine(AppContext.BaseDirectory, "XmlDocumentation.xml");
                c.IncludeXmlComments(filePath);
            });
        }
       
        /// <summary>
        /// ��������� ��������� HTTP-��������.
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "messageServer1 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
