using codeessentials.SemanticKernel.PromptTemplates.Git;

namespace Microsoft.SemanticKernel;

/// <summary>
/// Extensions methods for registering the Git prompt templates service in the <see cref="IKernelBuilder"/>.
/// </summary>
public static class GitPromptTemplatesKernelBuilderExtensions
{
	/// <summary>
	/// Adds Git prompt templates to the service collection using the specified repository configuration.
	/// </summary>
	/// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
	/// <param name="repositoryConfiguration">A delegate to configure the <see cref="GitPromptRepositoryConfiguration"/> used to set up the Git prompt templates.</param>
	/// <returns>The same instance as <paramref name="builder"/>.</returns>
	public static IKernelBuilder AddGitPromptTemplates(this IKernelBuilder builder, Action<GitPromptRepositoryConfiguration> repositoryConfiguration)
	{
		builder.Services.AddGitPromptTemplates(repositoryConfiguration);

		return builder;
	}

	/// <summary>
	/// Adds Git prompt templates to the service collection using the specified repository configuration.
	/// </summary>
	/// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
	/// <param name="repositoryConfiguration">A  <see cref="GitPromptRepositoryConfiguration"/> used to set up the Git prompt templates.</param>
	/// <returns>The same instance as <paramref name="builder"/>.</returns>
	public static IKernelBuilder AddGitPromptTemplates(this IKernelBuilder builder, GitPromptRepositoryConfiguration repositoryConfiguration)
	{
		builder.Services.AddGitPromptTemplates(repositoryConfiguration);

		return builder;
	}
}