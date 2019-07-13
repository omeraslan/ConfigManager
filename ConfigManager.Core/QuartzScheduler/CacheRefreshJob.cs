using ConfigManager.Core.Contracts;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConfigManager.Core.QuartzScheduler
{
    public class CacheRefreshJob : IJob
    {
        private readonly IConfigurationEngine _configurationEngine;

        public CacheRefreshJob(IConfigurationEngine configurationEngine)
        {
            _configurationEngine = configurationEngine;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _configurationEngine.RefreshCache();
            await Task.CompletedTask;
        }
    }
}
