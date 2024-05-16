using Data_Integration;
using Data_Integration.Services.ProductDetails;
using Data_Integration.Services.RabbitMQ;
using DeliveryIntegration.Configrations;
using DeliveryIntegration.Services.HttpRequest;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

InjectService();
SetConfigurations();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();

void InjectService()
{
    builder.Services.AddHttpClient<IHttpRequestService, HttpRequestService>();
    builder.Services.AddTransient<IProductServices, ProductServices>();
    builder.Services.AddHostedService<Consumer>();
}
void SetConfigurations()
{
    builder.Services.Configure<RabbitMQConfig>(builder.Configuration.GetSection(nameof(RabbitMQConfig)));

}


