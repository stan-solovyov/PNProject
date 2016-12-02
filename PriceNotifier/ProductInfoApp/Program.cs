using Autofac;

namespace ProductInfoApp
{
    class Program
    {
        static void Main()
        {
            var container = ContainerConfig.Configure();
            while (true)
            {
                container.BeginLifetimeScope().Resolve<IProductInfoService>();
            }
        }
    }
}
