using System;
using Autofac;

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
