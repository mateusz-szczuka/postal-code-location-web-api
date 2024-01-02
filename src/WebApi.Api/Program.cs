using WebApi.Core.Components;
using WebApi.Core.Middlewares;
using MediatR;
using WebApi.Core.Generators;
using WebApi.Core;
using WebApi.Core.Options;
using WebApi.Core.Services;
using WebApi.Core.Clients.Abstractions;
using WebApi.Core.Generators.Abstractions;
using Carter;
using WebApi.Core.Components.Abstractions;
using WebApi.Core.Clients;

const string ConfigPath = "Configs/appConfig.json";

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(ConfigPath, optional: false);

builder.Services
    .AddMediatR(AssemblyCoreReference.Assembly)
    .AddCarter()
    .AddHttpClient()
    .AddMemoryCache()
    .AddSingleton<IPostalCodesGenerator, PostalCodesGenerator>()
    .AddTransient<IPolicyFactory, PolicyFactory>()
    .AddTransient<IHttpRequestClient, HttpRequestClient>()
    .AddTransient<IZippoIntegrationComponent, ZippoIntegrationComponent>()
    .Decorate<IZippoIntegrationComponent, CachedZippoIntegrationComponent>()
    .AddHostedService<CachedPostalCodeLocationsService>();

builder.Services.AddOptionsWithValidateOnStart<PostalCodeGeneratorOptions>()
    .Bind(builder.Configuration.GetSection(nameof(PostalCodeGeneratorOptions)))
    .ValidateDataAnnotations();

builder.Services
    .AddOptions<CachedPostalCodeLocationsOptions>()
    .Bind(builder.Configuration.GetSection(nameof(CachedPostalCodeLocationsOptions)));

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.MapCarter();

var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger<Program>();

try
{
    await app.RunAsync();
}
catch (Exception ex)
{
    var error = $"{nameof(Program)}: Critical error occured. {ex.Message}";
    logger.LogCritical(ex, error);
}
