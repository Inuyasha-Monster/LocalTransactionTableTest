using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Consumer.EasyNetQ;
using Consumer.Mongo;
using Consumer.Option;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Consumer
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
            services.AddAutoMapper(x =>
            {
                x.AllowNullCollections = true;
            });

            services.AddMvc();

            services.Configure<MongoConfig>(x =>
            {
                x.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                x.Database = Configuration.GetSection("MongoConnection:Database").Value;
            });

            services.AddSingleton<MongoDbContext>();

            services.AddEasyNetQ(Configuration.GetConnectionString("RabbitMq"));
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
            app.UseMvc();
            app.UseEasyNetQ();
        }
    }
}
