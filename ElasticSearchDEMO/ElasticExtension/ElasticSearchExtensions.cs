using Elasticsearch.Net;
using ElasticSearchDEMO.Models;
using Nest;

namespace ElasticSearchDEMO.ElasticExtension
{
    public static class ElasticSearchExtension
    {

        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration["ELKConfiguration:Url"];
            var index = configuration["ELKConfiguration:Index"];

            var settings = new ConnectionSettings(new Uri(baseUrl ?? "")).PrettyJson()
                .DefaultIndex(index);
            AddDefaultMappings(settings);
            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);
            CreateIndex(client, index);
            var indexExistsResponse = client.Indices.Exists(index);
        }
        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings.DefaultMappingFor<Product>(
            p => p
            // p.Ignore(p => p.Price)

            //.Ignore(p => p.Quantity)
            //.Ignore(i=>i.Id)
            );
        }
        private static void CreateIndex(IElasticClient client, string indexName)
        {
            try
            {
                var createIndexResponse = client.Indices.Create(indexName, index => index.Map<Product>(x => x.AutoMap()));
            }
            catch (ElasticsearchClientException e)
            {
                // Inspect the detailed error response
                var response = e.Response;
                var responseBody = response?.DebugInformation;
                Console.WriteLine(responseBody);
                // Add additional logging or error handling here
            }
            catch (Exception ex) { }
        }
    }
}
