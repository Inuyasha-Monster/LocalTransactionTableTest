using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Productor.Quartz
{
    /// <summary>
    /// 清理历史消息记录保证消息表不会过载
    /// </summary>
    [IgnoreJob]
    public class ClearHistoryMessageJob : JobBase
    {
        private readonly ILogger<ClearHistoryMessageJob> _logger;

        public ClearHistoryMessageJob(ILogger<ClearHistoryMessageJob> logger)
        {
            _logger = logger;
        }

        protected override ILogger Logger => _logger;
        protected override async Task ExecuteJob(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
