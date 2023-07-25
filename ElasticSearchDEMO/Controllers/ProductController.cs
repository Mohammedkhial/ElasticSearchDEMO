using ElasticSearchDEMO.Models;
using ElasticSearchDEMO.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ElasticSearchDEMO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IElasticsearchRepository _elasticsearchRepository;
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IElasticClient elasticClient,ILogger<ProductController> logger, IElasticsearchRepository elasticsearchRepository)
        {
            _elasticsearchRepository = elasticsearchRepository;
            _elasticClient = elasticClient;
            _logger = logger;
        }
        [HttpGet(Name ="GetProducts")]
        public async Task<IActionResult> Get(string keyword)
        {
            var results = await _elasticClient.SearchAsync<Product>(a=>a
              //.Query(
              //    q=>q.QueryString(
              //        d=>d.Query('*'+keyword+'*')

              //        )
              //    ).Size(1000)
              .Query(q => q
                .Match(m => m
                    .Field(f => f.Title)
                    .Query(keyword)
                )
            )

            );

            return Ok(results.Documents.ToList());
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(List<Product> products)
        {
            await _elasticsearchRepository.InsertProducts(products);
            //await _elasticClient.IndexDocumentAsync<Product>(product);
            return Ok();
        }
        [HttpPost("BulkInsert")]
        public async Task<IActionResult> BulkInsert(List<Product> products)
        {
            await _elasticsearchRepository.BulkInsertProducts(products);
            return Ok();
        }
        [HttpPut("UpdateBulkProducts")]
        public async Task<IActionResult> UpdateBulkProducts(List<Product> products)
        {
            await _elasticsearchRepository.SyncDataToElasticsearch(products);
            return Ok();
        }
        [HttpDelete("DeleteProductById")]
        public async Task<IActionResult> DeleteProductById(int Id) {
            await _elasticsearchRepository.DeleteProductById(Id);
            return Ok();
        }

    }
}
