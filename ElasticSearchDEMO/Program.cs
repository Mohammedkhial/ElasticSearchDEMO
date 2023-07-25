using ElasticSearchDEMO.ElasticExtension;
using ElasticSearchDEMO.Repos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddElasticSearch(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IElasticsearchRepository, ElasticsearchRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseSwagger();


app.UseSwaggerUI(options =>
{
    options.DocumentTitle = "Elastic API";
  
});
app.Run();
