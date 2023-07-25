using ElasticSearchDEMO.Models;
using Nest;

namespace ElasticSearchDEMO.Repos
{
    public class ElasticsearchRepository : IElasticsearchRepository
    {
        //private readonly ElasticClient _client;
        private readonly IElasticClient _elasticClient;

        public ElasticsearchRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
            //var settings = new ConnectionSettings(new Uri(connectionString))
            //    .DefaultIndex("products"); // Replace "products" with your index name

            //_client = new ElasticClient(settings);
        }
        public async Task<bool> InsertProducts(List<Product> products)
        {
            var indexResponse =await _elasticClient.IndexManyAsync(products);
            
            if (!indexResponse.IsValid)
            {
                // Handle the case when the index response is not valid
                throw new Exception("Failed to insert products into Elasticsearch.");
            }
            return true;
        }


        public async Task<bool> SyncDataToElasticsearch(List<Product> products)
        {
            var bulkDescriptor = new BulkDescriptor();

            foreach (var product in products)
            {
                bulkDescriptor.Index<Product>(i => i
                    .Index("product") // Replace "product" with your index name
                    .Document(product)
                    .Id(product.Id)
                );
            }

            var bulkResponse =await _elasticClient.BulkAsync(bulkDescriptor);

            if (!bulkResponse.IsValid)
            {
                // Handle the case when the bulk response is not valid
                throw new Exception("Failed to sync data to Elasticsearch.");
            }
            return true;
        }
        // Delete Documents from Elasticsearch Index based on a Query
        public async void DeleteDocumentsByQuery(string query)
        {
            var deleteResponse =await _elasticClient.DeleteByQueryAsync<Product>(s => s
                .Query(q => q
                    .QueryString(qs => qs
                        .Query(query)
                    )
                )
            );

            if (!deleteResponse.IsValid)
            {
                // Handle the case when the delete response is not valid
                throw new Exception("Failed to delete documents from Elasticsearch.");
            }
        }

        public async Task<bool> DeleteProductById(int productId)
        {
            var deleteResponse =await _elasticClient.DeleteAsync<Product>(productId, d => d
                .Index("product") // Replace "product" with your index name
            );

            if (!deleteResponse.IsValid)
            {
                // Handle the case when the delete response is not valid
                throw new Exception("Failed to delete the document from Elasticsearch.");
            }
            return true;
        }

        public async Task<bool> BulkInsertProducts(List<Product> products)
        {
            var bulkDescriptor = new BulkDescriptor();

            foreach (var product in products)
            {
                bulkDescriptor.Index<Product>(i => i
                    .Index("product") // Replace "products" with your index name
                    .Document(product)
                );
            }

            var bulkResponse =await _elasticClient.BulkAsync(bulkDescriptor);

            if (!bulkResponse.IsValid)
            {
                // Handle the case when the bulk response is not valid
                throw new Exception("Failed to perform bulk insert in Elasticsearch.");
            }
            return true;
        }
    }
}
