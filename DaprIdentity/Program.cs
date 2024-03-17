using Carter;
using DaprIdentity.IntegrationEvents;
using DaprIdentity.Modules;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnDapr.BuildingBlocks.EventBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDaprClient();
builder.Services.AddScoped<IDatabase, Database>();
builder.Services.AddScoped<IEventBus, DaprEventBus>();

builder.Services.AddCarter();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

// 订阅消息
app.UseCloudEvents();
app.MapSubscribeHandler();

//app.MapPost("/subscribe", () =>
//{
//    Console.WriteLine("我要哈哈哈！");
//}).WithTopic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToSubmittedIntegrationEvent));


app.MapCarter();
app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
