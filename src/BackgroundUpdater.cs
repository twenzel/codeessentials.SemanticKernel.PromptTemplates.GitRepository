
using Microsoft.Extensions.DependencyInjection;

namespace codeessentials.SemanticKernel.PromptTemplates.Git;

internal class BackgroundUpdater : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly GitPromptRepositoryConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;

    public BackgroundUpdater(GitPromptRepositoryConfiguration configuration, IServiceScopeFactory scopeFactory)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_configuration.UpdateCheckInterval == TimeSpan.Zero)
            return;

        using var timer = new PeriodicTimer(_configuration.UpdateCheckInterval);

        do
        {
            using var scope = _scopeFactory.CreateScope();
            var updater = scope.ServiceProvider.GetRequiredService<IGitPromptHandler>();
            await updater.UpdatePromptsAsync(stoppingToken);
        } while (await timer.WaitForNextTickAsync(stoppingToken));
    }
}
