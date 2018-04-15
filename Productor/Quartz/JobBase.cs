using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Productor.Quartz
{
    [DisallowConcurrentExecution]
    [PersistJobDataAfterExecution]
    public abstract class JobBase : IJob
    {
        protected abstract ILogger Logger { get; }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                return ExecuteJob(context);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, $"当前任务key:{context.JobDetail.Key};当前任务描述desc:{context.JobDetail.Description}执行出现未知异常");
                JobExecutionException jobExecutionException = new JobExecutionException(exception, refireImmediately: true);
                throw jobExecutionException;
            }
        }

        protected abstract Task ExecuteJob(IJobExecutionContext context);
    }
}
