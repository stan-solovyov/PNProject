namespace ElasticIndexBuilder
{
    public interface IElasticIndex
    {
        void BuildIndexes();
        void DeleteIndexes();
    }
}
