using System;
using Autofac;

namespace ElasticIndexBuilder
{
    class Program
    {
        static void Main()
        {
            var container = ContainerConfig.Configure();
            while (true)
            {
                Console.WriteLine("Please choose an option:");
                Console.WriteLine("1. Create new Elastic indexes");
                Console.WriteLine("2. Delete Elastic indexes");
                Console.WriteLine("3. Exit");
                var option = Console.ReadLine();
                Console.WriteLine("");
                switch (option)
                {
                    case "1":
                        using (var scope = container.BeginLifetimeScope())
                        {
                            var app = scope.Resolve<IElasticIndex>();
                            app.BuildIndexes();
                        }
                        break;
                    case "2":
                        using (var scope = container.BeginLifetimeScope())
                        {
                            var app = scope.Resolve<IElasticIndex>();
                            app.DeleteIndexes();
                        }
                        break;
                    case "3":
                        return;
                }
            }
        }
    }
}
