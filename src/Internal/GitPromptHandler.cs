using Microsoft.SemanticKernel;

namespace codeessentials.SemanticKernel.PromptTemplates.Git.Internal;

internal class GitPromptHandler : IGitPromptHandler
{
    private readonly IGitRepository _repository;
    private readonly GitPromptRepositoryConfiguration _repositoryConfiguration;

    public GitPromptHandler(IGitRepository repository, GitPromptRepositoryConfiguration repositoryConfiguration)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _repositoryConfiguration = repositoryConfiguration ?? throw new ArgumentNullException(nameof(repositoryConfiguration));
    }

    public KernelPlugin CreateKernelPlugin(Kernel kernel)
    {
        var repository = kernel.GetRequiredService<IGitRepository>();
        var tempDir = _repositoryConfiguration.GetLocalRepositoryPath();

        if (!Directory.Exists(tempDir))
            repository.Clone(tempDir);

        var path = string.IsNullOrEmpty(_repositoryConfiguration.Path)
            ? tempDir
            : Path.Combine(tempDir, _repositoryConfiguration.Path);

        var currentCommit = repository.GetCurrentCommit();

        var plugin = kernel.CreatePluginFromPromptDirectoryYaml(path, PromptGitKernelExtensions.GIT_PROMPT_PLUGIN_NAME, _repositoryConfiguration.PromptTemplateFactory);

        if (_repositoryConfiguration.DeleteTemporaryDirectory)
            Directory.Delete(tempDir, true);

        return plugin;
    }

    public Task UpdatePromptsAsync(CancellationToken cancellationToken = default)
    {
        _repository.UpdateRepository();

        return Task.CompletedTask;
    }
}
