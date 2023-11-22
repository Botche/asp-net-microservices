using Discount.API.Extensions;

using EventBus.Messages.Common;

using MassTransit;

using Ordering.API.EventBusConsumers;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<BasketCheckoutConsumer>();

    config.UsingRabbitMq((context, configRmq) =>
    {
        configRmq.Host(builder.Configuration.GetValue<string>("EventBusSettings:HostAddress"));

        configRmq.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueueName, configEndpoint =>
        {
            configEndpoint.ConfigureConsumer<BasketCheckoutConsumer>(context);
        });
    });
});

var app = builder.Build();

app.MigrateDatabase<OrderContext>((context, services) =>
{
    ILogger<OrderContextSeed> logger = services.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed
        .SeedAsync(context, logger)
        .Wait();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
