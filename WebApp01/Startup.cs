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
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApp01.Repositories;
using WebApp01.Services.Services;
using WebApp01.Shared.Interfaces;
using System.Data.SqlClient;
using System.Reflection;
using System.IO;
using Prometheus;
using WebApp01.Web.Prometheus;
using WebApp01.Prometheus;

using OpenTracing;
using OpenTracing.Contrib.NetCore.Configuration;
//using OpenTracing.Contrib.NetCore.CoreFx;
using OpenTracing.Util;

using Jaeger;
using Jaeger.Samplers;
using WebApp01.Web.Jaeger;

namespace WebApp01
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            //Configura o AutoMapper para Scanear o Assembly para encontrar Profiles
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Configura o Swagger
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApp01", Version = "v1" });
                
                //Busca a documentacao gerada no XML e aplica no Swagger
                //   Tem que habilitar no Projeto
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                config.IncludeXmlComments(xmlCommentsFullPath);
            });

            //Injeção de Dependencia
            services.AddTransient<IPersonService, PersonService>();
            services.AddTransient<IPersonRepository, PersonRepository>();
            services.AddSingleton<MetricReporter>();

            //Adiciona o HealthCheck 
            services.AddHealthChecks()
                .AddSqlServer(Configuration.GetConnectionString("WebApp01DataBase")); // Adiciona Verificação do Banco De dados

            //Adicionando o Jaeger
            //tem que instalar os 3 pacotes abaixo
            //   Jaeger
            //   OpenTracing
            //   OpenTracing.Contrib.NetCore
            services.AddTransient<InjectOpenTracingHeaderHandler>();
            services.AddHttpClient("WebJaegerTest", c =>
            {
                c.BaseAddress = new Uri("https://localhost:5011/api");
            })
            .AddHttpMessageHandler<InjectOpenTracingHeaderHandler>();
            services.AddSingleton<ITracer>(serviceProvider =>
            {
                var serviceName = serviceProvider
                    .GetRequiredService<IWebHostEnvironment>()
                    .ApplicationName;
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var tracer = new Tracer.Builder(serviceName)
                    .WithSampler(new ConstSampler(true))
                    .WithLoggerFactory(loggerFactory)
                    .Build();
                // Allows code that can't use DI to also access the tracer.
                GlobalTracer.Register(tracer);
                return tracer;
            });
            services.Configure<HttpHandlerDiagnosticOptions>(options =>
            {
                options.IgnorePatterns.Add(x => !x.RequestUri.IsLoopback);
            });


        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               
            }

            //Pipeline do Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApp01 v1"));

            

            app.UseHttpsRedirection();

            app.UseRouting();

            //Adicionando o Prometheus
            //   - tem que instalar os pacotes abaixo
            //        Install-Package prometheus-net
            //        Install-Package prometheus-net.AspNetCore
            //  e usar o middleware app.UseMetricServer();    
            // link - Prometheus - https://localhost:5001/metrics
            app.UseMetricServer();
            app.UseMiddleware<ResponseMetricMiddleware>(); //Middleware para computar o tempo dos requests

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                //Endereço do Health Check
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
