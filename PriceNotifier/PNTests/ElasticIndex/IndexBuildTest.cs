using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Domain.EF;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreLinq;
using Nest;
using System.Diagnostics;

namespace PNTests.ElasticIndex
{
    /// <summary>
    /// Summary description for IndexBuildTest
    /// </summary>
    [TestClass]
    public class IndexBuildTest
    {
        private readonly UserContext _db = new UserContext();
        private ElasticClient client;
        private string indexName;
        private IndexName index;

        [TestInitialize]
        public void Initialize()
        {
            indexName = "indextest";
            index = new IndexName { Name = indexName };
            var local = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(local);
            client = new ElasticClient(settings.DefaultIndex(indexName));
            client.CreateIndex(index);
        }

        [TestMethod]
        public void AddToIndexBatch()
        {
            //Arrange
            var watch = Stopwatch.StartNew();

            //Act
            var products = _db.Products.AsNoTracking().Include(d => d.Articles).Include(d => d.ProvidersProductInfos).Include(d => d.UserProducts).OrderBy(t => t.Id).Batch(5000);
            var enumerable = products as IList<IEnumerable<Product>> ?? products.ToList();
            foreach (var batch in enumerable)
            {
                Trace.WriteLine("Trace Information-Indexing Starting ");
                Trace.Indent();
                watch.Restart();
                var took = client.IndexMany(batch).TookAsLong;
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Trace.WriteLine("Took to build from native tool,ms: " + took);
                Trace.WriteLine("Took to build from stopwatch,ms: " + elapsedMs);
                Trace.Unindent();
                Trace.WriteLine("Trace Information-Indexing Ending");
                Trace.Flush();
            }
            client.DeleteIndexAsync(index);
        }

        [TestMethod]
        public void AddToIndex()
        {
            //Arrange
            var watch = Stopwatch.StartNew();

            //Act
            var products = _db.Products.AsNoTracking().Include(d => d.Articles).Include(d => d.ProvidersProductInfos).Include(d => d.UserProducts).OrderBy(t => t.Id).Batch(5000);
            var enumerable = products as IList<IEnumerable<Product>> ?? products.ToList();
            
            Trace.WriteLine("Trace Information-Indexing Starting ");
            Trace.Indent();
            foreach (var batch in enumerable)
            {
                foreach (var p in batch)
                {
                    watch.Restart();
                    client.Index(p, x => x.Id(p.Id));
                    //Thread.Sleep(3000);
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    Trace.WriteLine("");
                    Trace.WriteLine(elapsedMs);
                }
            }
            Trace.Unindent();
            Trace.WriteLine("Trace Information-Indexing Ending");
            Trace.Flush();
            client.DeleteIndexAsync(index);
        }

        [TestMethod]
        public void AddToIndexRefresh()
        {
            //Arrange
            var watch = Stopwatch.StartNew();

            //Act
            var products = _db.Products.AsNoTracking().Include(d => d.Articles).Include(d => d.ProvidersProductInfos).Include(d => d.UserProducts).OrderBy(t => t.Id).Batch(5000);
            var enumerable = products as IList<IEnumerable<Product>> ?? products.ToList();

            Trace.WriteLine("Trace Information-Indexing Starting ");
            Trace.Indent();
            foreach (var batch in enumerable)
            {
                foreach (var p in batch)
                {
                    watch.Restart();
                    client.Index(p, x => x.Id(p.Id).Refresh());
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    Trace.WriteLine("");
                    Trace.WriteLine(elapsedMs);
                }
            }
            Trace.Unindent();
            Trace.WriteLine("Trace Information-Indexing Ending");
            Trace.Flush();
            client.DeleteIndexAsync(index);
        }
    }
}