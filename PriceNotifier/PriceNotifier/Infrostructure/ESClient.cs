using System;
using System.Linq;
using Domain.Entities;
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

        public static IQueryable<Product> SearchProducts(string query)
        {
            var client = ElasticClient;
            return client.Search<Product>(s => s
                .From(0)
                .Size(1000)
                .Query(
                        q => q.Wildcard(wc => wc.Field(f => f.Name).Value("*" + query + "*")) || q.Term(d => d.Name, "*" + query + "*") || q.Match(d => d.Field(f => f.Name).Query(query)) || q.QueryString(a => a.Query(query)))
                ).Documents.AsQueryable();
        }
    }
}