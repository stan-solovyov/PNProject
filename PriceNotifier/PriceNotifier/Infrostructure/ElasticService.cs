using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities;
using Nest;

namespace PriceNotifier.Infrostructure
{
    public class ElasticService<T> : IElasticService<T> where T : class,IEntityWithTypedId<int>
    {
        private readonly ElasticClient _client;
        private readonly string _indexName;
        private readonly Expression<Func<T, object>> _lambda;

        protected ElasticService(string elastiSearchServerUrl, string indexName, Expression<Func<T, object>> lambda)
        {
            if (string.IsNullOrEmpty(elastiSearchServerUrl))
            {
                throw new Exception("elastiSearchServerUrl should not be null!");
            }
            _indexName = indexName;
            _lambda = lambda;
            var local = new Uri(elastiSearchServerUrl);
            var settings = new ConnectionSettings(local);
            _client = new ElasticClient(settings.DisableDirectStreaming().DefaultIndex(_indexName));
        }
        public IQueryable<T> SearchProducts(string query)
        {
            return _client.Search<T>(s => s
                .From(0)
                .Size(1000)
                .Query(
                        q => q.Wildcard(wc => wc.Field(_lambda).Value("*" + query + "*")) || q.Term(_lambda, "*" + query + "*") || q.Match(d => d.Field(_lambda).Query(query)) || q.QueryString(a => a.Query(query)))
                ).Documents.AsQueryable();
        }

        public Task DeleteIndex()
        {
            var index = new IndexName { Name = _indexName };
            return _client.DeleteIndexAsync(index);
        }

        public void DeleteFromIndex(int id)
        {
            _client.DeleteAsync<T>(id);
        }

        public void AddToIndex(T doc, int id)
        {
            _client.Index(doc, idx => idx.Id(id));
        }

        public IUpdateResponse<T> UpdateDoc(int id, T doc)
        {
            var a = _client.Get<Product>(id); // returns an IGetResponse mapped 1-to-1 with the Elasticsearch JSON response
            var response = _client.Update(new DocumentPath<T>(id), u => u.Doc(doc));
            return response;
        }

        public void AddToIndexMany(IEnumerable<T> docs)
        {
            var descriptor = new BulkDescriptor();

            foreach (var i in docs)
            {
                descriptor.Index<T>(op => op
                .Id(i.Id)
                .Document(i)
                );
            }

            _client.Bulk(descriptor);
        }
    }
}