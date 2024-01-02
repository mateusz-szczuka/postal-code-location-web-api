using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApi.Core.Extensions;

namespace WebApi.Api.Services
{
	public abstract class BaseWorkerService : BackgroundService
	{
        protected readonly ILogger _logger;

        private readonly IServiceProvider _serviceProvider;

        public BaseWorkerService(
            IServiceProvider serviceProvider,
            ILogger logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected IServiceScope CreateScope() => _serviceProvider.CreateScope();

        protected R GetRequiredService<R>(IServiceScope scope)
            where R : class
        {
            if (scope.IsNull())
                throw new ArgumentNullException(nameof(scope));

            return scope.ServiceProvider.GetRequiredService<R>();
        }

        protected R GetService<R>(IServiceScope scope)
            where R : class
        {
            if (scope.IsNull())
                throw new ArgumentNullException(nameof(scope));

            return scope.ServiceProvider.GetService<R>();
        }
    }
}

