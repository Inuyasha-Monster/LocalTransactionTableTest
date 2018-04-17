using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
using Quartz.Spi;
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace Productor.Quartz
{
    public static class QuartzExtension
    {
        private static void InternalInitScheduer(IServiceCollection service, NameValueCollection props)
        {
            StdSchedulerFactory factory = new StdSchedulerFactory(props);

            // registe scheduerfactory
            service.AddSingleton<ISchedulerFactory>(factory);

            // get a scheduler
            IScheduler scheduler = factory.GetScheduler().Result;

            // register cutomer jobfacotry
            service.AddSingleton<IJobFactory, ProductorJobFactory>(provider => new ProductorJobFactory(provider.GetService<ILogger<ProductorJobFactory>>(), provider));

            // registe all jobs to Iservicecollection
            var jobs = typeof(JobBase).Assembly.GetTypes().Where(x => x.BaseType == typeof(JobBase) || x.GetInterfaces().Any(i => i == typeof(IJob))).Where(x => x.GetCustomAttribute<IgnoreJobAttribute>() == null)
                .Where(x => x.IsAbstract == false);

            foreach (var job in jobs)
            {
                service.AddTransient(job);
                service.AddTransient(typeof(IJob), job);
            }

            // registe scheduer
            service.AddSingleton<IScheduler>(scheduler);
        }

        public static void AddQuartz(this IServiceCollection service, NameValueCollection props)
        {
            InternalInitScheduer(service, props);
        }

        public static void AddQuartz(this IServiceCollection service)
        {
            // construct a scheduler factory
            NameValueCollection props = new NameValueCollection
            {
                { "quartz.serializer.type", "json" },
                { "quartz.scheduler.instanceName", "ProductorScheduler" },
                { "quartz.jobStore.type", "Quartz.Simpl.RAMJobStore, Quartz" },
                { "quartz.threadPool.threadCount", "3" }
            };
            InternalInitScheduer(service, props);
        }

        public static void AddQuartzUI()
        {

        }

        public static void UseQuartz(this IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices;
            var scheduler = serviceProvider.GetRequiredService<IScheduler>();
            var jobFactory = serviceProvider.GetRequiredService<IJobFactory>();
            scheduler.JobFactory = jobFactory;
            scheduler.Start().Wait();

            // add jobdetail with trigger to scheduler 
            var jobs = app.ApplicationServices.GetServices<IJob>();

            foreach (var job in jobs)
            {
                var jobDesc = job.GetType().GetCustomAttribute<JobDescriptionAttribute>();
                var jobBuilder = JobBuilder.Create(job.GetType());
                if (jobDesc != null)
                {
                    jobBuilder.WithIdentity(jobDesc.Key, jobDesc.Group).WithDescription(jobDesc.Description);
                }
                var jobDetail = jobBuilder.Build();

                ITrigger trigger;
                var triggerDesc = job.GetType().GetCustomAttribute<JobIntervalTriggerAttribute>();
                if (triggerDesc == null)
                {
                    // default trigger
                    trigger = TriggerBuilder.Create().StartNow()
                        .WithSimpleSchedule(x => x.WithIntervalInSeconds(60).RepeatForever()).Build();
                }
                else
                {
                    var temp = TriggerBuilder.Create().WithIdentity(triggerDesc.Key, triggerDesc.Group);
                    if (triggerDesc.StartNow)
                    {
                        temp = temp.StartNow();
                    }
                    if (triggerDesc.IsRepeatForever)
                    {
                        temp = temp.WithSimpleSchedule(x => x.WithIntervalInSeconds(triggerDesc.IntervalInSeconds)
                            .RepeatForever());
                    }
                    else
                    {
                        temp = temp.WithSimpleSchedule(x => x.WithIntervalInSeconds(triggerDesc.IntervalInSeconds)
                            .WithRepeatCount(triggerDesc.RepeatCount));
                    }
                    trigger = temp.Build();
                }
                scheduler.ScheduleJob(jobDetail, trigger).Wait();
            }
        }

        public static void UseQuartzUI()
        {

        }
    }
}
