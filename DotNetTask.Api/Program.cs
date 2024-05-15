using DotNetTask.Services.Interfaces;
using DotNetTask.Services.Services;
using Microsoft.Azure.Cosmos;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Adding the CosmosDb Connection
    builder.Services.AddSingleton(ServiceProvider =>
    {
        var endpointUri = builder.Configuration["CosmosDb:EndpointUri"];
        var priKey = builder.Configuration["CosmosDb:PrimaryKey"];
        var dbName = builder.Configuration["CosmosDb:DatabaseName"];

        var cosmosClientOptions = new CosmosClientOptions
        {
            ApplicationName = dbName
        };

        logger.Info("----------------------- Cosmos DB successfully instantiated ----------------------------");

        var cosmosClient = new CosmosClient(endpointUri, priKey, cosmosClientOptions);
        cosmosClient.ClientOptions.ConnectionMode = ConnectionMode.Gateway;

        return cosmosClient;
    });

    // Add services to the container.

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddControllers();

    // Services are injected here to be available app wide
    builder.Services.AddScoped<ICandidateService, CandidateService>();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

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
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
