using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Simpl;
using Quartz.Spi;

namespace Productor.Quartz
{
    public class ProductorJobFactory : IJobFactory
    {
        private readonly ILogger<ProductorJobFactory> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ProductorJobFactory(ILogger<ProductorJobFactory> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                var job = this._serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
                if (job == null)
                    throw new ArgumentNullException($"{nameof(job)}");
                return job;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Problem while instantiating job '{bundle.JobDetail.Key}' from the ProductorJobFactory.");
                throw new SchedulerException($"Problem while instantiating job '{bundle.JobDetail.Key}' from the ProductorJobFactory.", exception);
            }
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
