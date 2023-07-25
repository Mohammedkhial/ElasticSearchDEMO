using ElasticSearchDEMO.Models;

namespace ElasticSearchDEMO.Repos
{
    public interface IElasticsearchRepository
    {
        public Task<bool> InsertProducts(List<Product> products);
        public Task<bool> SyncDataToElasticsearch(List<Product> products);
        public  Task<bool> DeleteProductById(int productId);
        public Task<bool> BulkInsertProducts(List<Product> products);
    }
}
