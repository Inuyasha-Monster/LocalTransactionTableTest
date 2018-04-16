using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Productor.Common;
using Productor.Data;
using Productor.EasyNetQ;
using Productor.Filter;
using Productor.Interceptor;
using Productor.Quartz;
using Productor.Service;
using Productor.Swagger;
using Swashbuckle.AspNetCore.Swagger;

namespace Productor
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(env.ContentRootPath)
            //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            //    .AddEnvironmentVariables();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(x =>
            {
                x.AllowNullCollections = true;
            });

            var mvcBuilder = services.AddMvc(x =>
            {
                //x.Filters.Add<ExceptionFilterAttribute>();
                x.Filters.Add<GlobalExceptionFilter>();
                x.Filters.Add<ValidateModelStateAttribute>();
            }).AddControllersAsServices();

            mvcBuilder.AddJsonOptions(x =>
            {
                x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                x.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                x.SerializerSettings.DateFormatString = "yyyy-MM-dd hh:mm:ss";
            });

            services.AddSwagger();

            services.AddDbContext<ProductDbContext>(builderDb =>
            {
                //var connectionStr = Configuration.GetConnectionString("Mysql");
                var connectionStr = Configuration.GetConnectionString("LocalMysql");
                builderDb.UseMySql(connectionStr);
            });


            //添加你的服务注册到services...
            //services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped<IOrderService, OrderService>();

            services.AddEasyNetQ(Configuration.GetConnectionString("RabbitMq"));

            services.AddSingleton<IEventBus, RabbitMqEventBus>();

            services.AddQuartz();

            //将IServiceCollection的服务添加到ServiceContainer容器中
            var container = services.ToServiceContainer();

            container.Configure(config =>
            {
                config.Interceptors.AddTyped<LogExceptionAspectCoreInterceptorAttribute>(method => method.DeclaringType.Name.EndsWith("Service"));
            });

            return container.Build();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddConsole(LogLevel.Trace);
            loggerFactory.AddLog4Net("log4net.config");

            app.UseStaticFiles();

            app.UseSwaggerAll();

            app.UseMvc();

            app.UseEasyNetQ();

            app.UseQuartz();

            lifetime.ApplicationStarted.Register(() =>
            {

            });

            lifetime.ApplicationStopping.Register(() =>
            {

            });

            lifetime.ApplicationStopped.Register(() =>
            {

            });
        }
    }
}
