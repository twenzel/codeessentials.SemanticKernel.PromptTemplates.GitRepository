using codeessentials.SemanticKernel.PromptTemplates.Git;
using codeessentials.SemanticKernel.PromptTemplates.Git.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.SemanticKernel;

/// <summary>
/// Extensions methods for registering the Git prompt templates service in the <see cref="IServiceCollection"/>.
/// </summary>
public static class GitPromptTemplatesServiceCollectionExtensions
{
	/// <summary>
	/// Adds Git prompt templates to the service collection using the specified repository configuration.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to which the Git prompt templates will be added.</param>
	/// <param name="repositoryConfiguration">A delegate to configure the <see cref="GitPromptRepositoryConfiguration"/> used to set up the Git prompt templates.</param>
	/// <returns>The <see cref="IServiceCollection"/> with the Git prompt templates added.</returns>
	public static IServiceCollection AddGitPromptTemplates(this IServiceCollection services, Action<GitPromptRepositoryConfiguration> repositoryConfiguration)
	{
		var config = new GitPromptRepositoryConfiguration();
		repositoryConfiguration(config);

		return services.AddGitPromptTemplates(config);
	}

	/// <summary>
	/// Adds Git prompt templates to the service collection using the specified repository configuration.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to which the Git prompt templates will be added.</param>
	/// <param name="repositoryConfiguration">A  <see cref="GitPromptRepositoryConfiguration"/> used to set up the Git prompt templates.</param>
	/// <returns></returns>
	public static IServiceCollection AddGitPromptTemplates(this IServiceCollection services, GitPromptRepositoryConfiguration repositoryConfiguration)
	{
		ArgumentNullException.ThrowIfNull(repositoryConfiguration);
		repositoryConfiguration.Validate();

		services.AddSingleton(repositoryConfiguration);
		services.AddScoped<IGitPromptHandler, GitPromptHandler>();
		services.AddScoped<IGitRepository, GitRepository>();

		services.AddHostedService<BackgroundUpdater>();

		return services;
	}
}