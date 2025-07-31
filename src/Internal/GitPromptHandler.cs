using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace codeessentials.SemanticKernel.PromptTemplates.Git.Internal;

internal class GitPromptHandler : IGitPromptHandler
{
	private readonly IGitRepository _repository;
	private readonly GitPromptRepositoryConfiguration _repositoryConfiguration;
	private readonly ILoggerFactory _loggerFactory;

	public GitPromptHandler(IGitRepository repository, GitPromptRepositoryConfiguration repositoryConfiguration, ILoggerFactory loggerFactory)
	{
		_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		_repositoryConfiguration = repositoryConfiguration ?? throw new ArgumentNullException(nameof(repositoryConfiguration));
		_loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
	}

	public KernelPlugin CreateKernelPlugin()
	{
		var currentCommit = UpdatePrompts(false);

		var plugin = CreatePlugin(PromptGitKernelExtensions.GIT_PROMPT_PLUGIN_NAME, currentCommit, _repositoryConfiguration.PromptTemplateFactory);

		return plugin;
	}

	private KernelPlugin CreatePlugin(string pluginName, string? description, IPromptTemplateFactory? promptTemplateFactory)
	{
		var functions = new List<KernelFunction>();
		var logger = _loggerFactory.CreateLogger<GitPromptHandler>();

		foreach (var item in TemplateRepository.Items)
		{
			if (logger.IsEnabled(LogLevel.Trace))
				logger.LogTrace("Registering function {PluginName}.{FunctionName} loaded from {FileName}", pluginName, item.Name, item.FileName);

			functions.Add(KernelFunctionFactory.CreateFromPrompt(item.Template, promptTemplateFactory, _loggerFactory));
		}

		return KernelPluginFactory.CreateFromFunctions(pluginName, description, functions);
	}

	public Task UpdatePromptsAsync(CancellationToken cancellationToken = default)
	{
		UpdatePrompts(true);

		return Task.CompletedTask;
	}

	public PromptTemplateConfig GetTemplate(string name)
	{
		UpdatePrompts(false);

		var template = TemplateRepository.Items.FirstOrDefault(x => x.Name == name)
			?? throw new ArgumentException($"No template found with name '{name}'!", nameof(name));

		return template.Template;
	}

	private string UpdatePrompts(bool forceUpdate)
	{
		var tempDir = _repositoryConfiguration.GetLocalRepositoryPath();

		if (!Directory.Exists(tempDir))
			_repository.Clone(tempDir);
		else if (forceUpdate)
			_repository.UpdateRepository();

		if (TemplateRepository.Items.Count == 0)
		{
			var path = string.IsNullOrEmpty(_repositoryConfiguration.Path)
					? tempDir
					: Path.Combine(tempDir, _repositoryConfiguration.Path);

			var logger = _loggerFactory.CreateLogger<GitPromptHandler>();

			TemplateRepository.UpdateItems(path, logger);
		}

		var currentCommit = _repository.GetCurrentCommit();

		if (_repositoryConfiguration.DeleteTemporaryDirectory)
			Directory.Delete(tempDir, true);

		return currentCommit;
	}
}
