using kafka_produce.Models;
using kafka_produce.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IProducerService, ProducerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/produce", async (Customer customer, IProducerService producerService) =>
{
    var result = await producerService.ProduceAsync(customer.Name);
    return Results.Accepted();
})
.WithName("New Customer")
.WithOpenApi();


app.Run();
