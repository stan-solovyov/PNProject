using System;
using Nest;

namespace PriceNotifier.Infrostructure
{
    public static class ESClient
    {
        public static ElasticClient ElasticClient
        {
            get
            {
                var node = new Uri("http://localhost:9200");
                var settings = new ConnectionSettings(node);
                settings.DefaultIndex("myindex");
                var client = new ElasticClient(settings);
                return  client;
            }
        }
    }
}