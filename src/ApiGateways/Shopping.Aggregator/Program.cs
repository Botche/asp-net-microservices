using Shopping.Aggregator.Services;
using Shopping.Aggregator.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<ICatalogService, CatalogService>(config =>
{
    config.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogUrl"]);
});
builder.Services.AddHttpClient<IBasketService, BasketService>(config =>
{
    config.BaseAddress = new Uri(builder.Configuration["ApiSettings:BasketUrl"]);
});
builder.Services.AddHttpClient<IOrderService, OrderService>(config =>
{
    config.BaseAddress = new Uri(builder.Configuration["ApiSettings:OrderingUrl"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
