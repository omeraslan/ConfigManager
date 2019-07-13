using System;
using Quartz;
using Quartz.Spi;

namespace ConfigManager.Core.QuartzScheduler
{
    public class ScheduledJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ScheduledJobFactory(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return _serviceProvider.GetService(typeof(IJob)) as IJob;
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
