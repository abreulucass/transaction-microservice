using System.Text.Json.Serialization;
using TransactionMicroservice.Application;
using TransactionMicroservice.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

// Testando o Conecção com Servicos Externos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await StartupDiagnostics.TestMongoConnectionAsync(services);
    await StartupDiagnostics.TestAzureServiceBusConnectionAsync(services);
}



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

