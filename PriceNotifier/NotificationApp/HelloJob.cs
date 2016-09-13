using System;
using Quartz;

namespace NotificationApp
{
    public class HelloJob:IJob
    {
        public async void Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync("Greetings from HelloJob!");
        }
    }
}
