using Elasticsearch.Net;
using ElasticSearchDEMO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ElasticSearchDEMO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IElasticClient _elasticClient;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ArticleController(IElasticClient elasticClient, IWebHostEnvironment hostingEnvironment)
        {
            _elasticClient = elasticClient;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody]ArticleModel model)
        {
            try
            {
                var article = new ArticleModel()
                {
                    Id = 0,
                    Title = model.Title,
                    Link = model.Link,
                    Author = model.Author,
                    AuthorLink = model.AuthorLink,
                    PublishedDate = DateTime.Now
                };
             var r =   await _elasticClient.IndexDocumentAsync(model);
                model = new ArticleModel();
                return Ok(r);

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
            return Ok(model);
        }
        [HttpGet("Index")]

        public IActionResult Index(string keyword)
        {
            var articleList = new List<ArticleModel>();
            if (!string.IsNullOrEmpty(keyword))
            {

                var result = _elasticClient.SearchAsync<ArticleModel>(s => s.Query(q => q.QueryString(d => d.Query('*' + keyword + '*'))).Size(5000));
                var finalResult = result;
                var finalContent = finalResult.Result.Documents.ToList();
               // articleList = GetSearch(keyword).ToList();
            }
            return Ok();
        }
        //public IList<ArticleModel> GetSearch(string keyword)
        //{
        //    var result = _elasticClient.SearchAsync<ArticleModel>(s => s.Query(q => q.QueryString(d => d.Query('*' + keyword + '*'))).Size(5000));
        //    var finalResult = result;
        //    var finalContent = finalResult.Result.Documents.ToList();
        //    return finalContent;
        //}
    }
}
