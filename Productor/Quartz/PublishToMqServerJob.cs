using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Productor.Common;
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

        public PublishToMqServerJob(ILogger<PublishToMqServerJob> logger, IEventBus eventBus)
        {
            _logger = logger;
            _eventBus = eventBus;
        }

        protected override ILogger Logger => _logger;

        protected override async Task ExecuteJob(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
