using Autofac;
using NotificationApp.JobTask;
using Quartz;
using Quartz.Impl;

namespace NotificationApp.JobSetup
{
    public class JobScheduler
    {
        public static void Start(IContainer container)
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = schedulerFactory.GetScheduler();
            scheduler.Start();

            scheduler.JobFactory = new AutofacJobFactory(container);
            IJobDetail job = JobBuilder.Create<PriceComparisonJob>().Build();

            ITrigger trigger = TriggerBuilder.Create().StartNow()
                .WithSimpleSchedule
                  (s =>
                     s.WithIntervalInMinutes(10)
                    .RepeatForever()
                  )
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
