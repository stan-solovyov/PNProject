using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Autofac;
using Quartz;
using Quartz.Impl;

namespace NotificationApp
{
    class Program
    {
        private static IContainer Container { get; set; }
        static void Main()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Parser>().As<IParser>();
            Container = builder.Build();

        }


        public interface IParser
        {
            string Parse();
        }

        public class Parser : IParser
        {
            public string Parse()
            {
                throw new NotImplementedException();
            }
        }

        public void UpdateProducts()
        {
            
        }
    }
}
