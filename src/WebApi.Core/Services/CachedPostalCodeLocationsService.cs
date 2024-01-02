using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApi.Api.Services;
using WebApi.Core.Components.Abstractions;
using WebApi.Core.Extensions;
using WebApi.Core.Generators.Abstractions;
using WebApi.Core.Options;

namespace WebApi.Core.Services
{
	public sealed class CachedPostalCodeLocationsService : BaseWorkerService
	{
        private const int MaxPackageSize = 100;

        private readonly TimeSpan DelayProcessingInSeconds = TimeSpan.FromSeconds(10);

        private readonly CachedPostalCodeLocationsOptions _cachedPostalCodeLocationsOptions;

        private readonly PeriodicTimer _timer;

        public CachedPostalCodeLocationsService(
            ILogger<CachedPostalCodeLocationsService> logger,
            IOptions<CachedPostalCodeLocationsOptions> cachedPostalCodeLocationsOptions,
            IServiceProvider serviceProvider) : base(serviceProvider, logger)
		{
            _cachedPostalCodeLocationsOptions = cachedPostalCodeLocationsOptions?.Value ?? throw new ArgumentNullException(nameof(cachedPostalCodeLocationsOptions));

            _timer = new PeriodicTimer(DelayProcessingInSeconds);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_cachedPostalCodeLocationsOptions.Enabled)
                return;

            try
            {
                int counter = 0;

                using var scope = CreateScope();
                var postalCodesGenerator = GetRequiredService<IPostalCodesGenerator>(scope);
                var zippoIntegrationComponent = GetRequiredService<IZippoIntegrationComponent>(scope);

                var postalCodes = postalCodesGenerator.GetAllPostalCodes();

                _logger.LogInformation($"{nameof(CachedPostalCodeLocationsService)}: I'm starting the cache process. PostalCodes: {postalCodes.Count}.");

                foreach (var postalCodePackage in postalCodes.Chunk(MaxPackageSize))
                {
                    try
                    {
                        var tasks = postalCodePackage.Select(s => zippoIntegrationComponent.GetLocationAsync(s));
                        var locationResults = await Task.WhenAll(tasks);
                        counter += locationResults.Count(x => x.CheckIfLocationResultIsCorrect());
                    }
                    catch (Exception ex)
                    {
                        var error = $"{nameof(CachedPostalCodeLocationsService)}: Failed to retrieve locations for postal code package: {string.Join(",", postalCodePackage)}.";
                        _logger.LogWarning(ex, error);
                    }
                    finally
                    {
                        await _timer.WaitForNextTickAsync();
                    }
                }

                _logger.LogInformation($"{nameof(CachedPostalCodeLocationsService)}: End of cache process. Cached {counter}/{postalCodes.Count} elements.");
            }
            catch (Exception ex)
            {
                var error = $"{nameof(CachedPostalCodeLocationsService)}: Error occurred - {ex.Message}";
                _logger.LogWarning(ex, error);
            }
        }
    }
}