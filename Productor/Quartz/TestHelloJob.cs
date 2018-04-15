using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Productor.Quartz
{
    public class TestHelloJob : JobBase
    {
        private readonly ILogger<TestHelloJob> _logger;

        public TestHelloJob(ILogger<TestHelloJob> logger)
        {
            _logger = logger;
        }

        protected override ILogger Logger => _logger;
        protected override Task ExecuteJob(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                Debug.WriteLine("Hello djlnet");
            });
        }
    }
}
