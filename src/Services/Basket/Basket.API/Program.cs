using System.Reflection;

using Basket.API.GrpcServices;
using Basket.API.Repositories;

using MassTransit;

using static Discount.Grpc.Protos.DiscountProtoService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<DiscountGrpcService>();

builder.Services.AddGrpcClient<DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration.GetValue<string>("GrpcSettings:DiscountUrl"));
});

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((context, configRmq) =>
    {
        configRmq.Host(builder.Configuration.GetValue<string>("EventBusSettings:HostAddress"));
    });
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
