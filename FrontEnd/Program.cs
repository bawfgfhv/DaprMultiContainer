using FrontEnd.Infrastructure.Repositories;
using FrontEnd.IntegrationEvents.EventHandling;
using FrontEnd.Model;
using FrontEnd.Services;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnDapr.BuildingBlocks.EventBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDaprClient(); // Ìí¼Ó´úÂë 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEventBus, DaprEventBus>();
builder.Services.AddScoped<OrderStatusChangedToSubmittedIntegrationEventHandler>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IBasketRepository, DaprBasketRepository>();
builder.Services.AddScoped<IIdentityService, IdentityService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseCloudEvents();

app.MapSubscribeHandler();
app.MapControllers();

app.Run();
