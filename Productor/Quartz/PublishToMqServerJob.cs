using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Productor.Quartz
{
    [JobDescriptionAttribute(Key = "PublishToMqServerJob", Group = "Productor")]
    [JobIntervalTriggerAttribute(Key = "PublishToMqServerJob_Trigger", Group = "Productor", IntervalInSeconds = 30, StartNow = true)]
    public class PublishToMqServerJob : JobBase
    {
        private readonly ILogger<PublishToMqServerJob> _logger;

        public PublishToMqServerJob(ILogger<PublishToMqServerJob> logger)
        {
            _logger = logger;
        }

        protected override ILogger Logger => _logger;

        protected override Task ExecuteJob(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
