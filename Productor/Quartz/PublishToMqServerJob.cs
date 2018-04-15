using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Productor.Common;
using Productor.Data;
using Quartz;

namespace Productor.Quartz
{
    [JobDescriptionAttribute(Key = "PublishToMqServerJob", Group = "Productor")]
    [JobIntervalTriggerAttribute(Key = "PublishToMqServerJob_Trigger", Group = "Productor", IntervalInSeconds = 30, StartNow = true)]
    [IgnoreJobAttribute]
    public class PublishToMqServerJob : JobBase
    {
        private readonly ILogger<PublishToMqServerJob> _logger;
        private readonly IEventBus _eventBus;
        private readonly IServiceProvider _serviceProvider;

        public PublishToMqServerJob(ILogger<PublishToMqServerJob> logger, IEventBus eventBus, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _eventBus = eventBus;
            _serviceProvider = serviceProvider;
        }

        protected override ILogger Logger => _logger;

        protected override async Task ExecuteJob(IJobExecutionContext context)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
                using (var trans = await dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var waits = await dbContext.MqMessages.Where(x => x.IsPublished == false).OrderByDescending(x => x.CreateTime).Take(10).ToListAsync();
                        foreach (var msg in waits)
                        {
                            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                            var assembly = assemblies.SingleOrDefault(x => x.GetName().Name == msg.MessageAssemblyName) ??
                                           AppDomain.CurrentDomain.Load(msg.MessageAssemblyName);
                            var type = assembly.GetType(msg.MessageClassFullName);
                            var publishMsg = JsonConvert.DeserializeObject(msg.Body, type);
                            _eventBus.Publish(type, publishMsg);
                            msg.IsPublished = true;
                        }
                        dbContext.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
