using System.Security.Cryptography;
using System.Text;
using Microsoft.SemanticKernel;

namespace codeessentials.SemanticKernel.PromptTemplates.Git;

/// <summary>
/// The configuration for a Git prompt repository.
/// </summary>
public class GitPromptRepositoryConfiguration
{
	/// <summary>
	/// Gets or sets the URL of the repository associated with the current entity.
	/// </summary>
	public string? RepositoryUrl { get; set; }

	/// <summary>
	/// Gets or sets the branch of the repository to be used.
	/// </summary>
	public string? Branch { get; set; }

	/// <summary>
	/// Gets or sets the path within the repository to the prompts directory.
	/// </summary>
	public string? Path { get; set; }

	/// <summary>
	/// Gets or sets the temporary directory where the repository will be cloned.
	/// </summary>
	public string? TemporaryDirectory { get; set; }

	/// <summary>
	/// Gets or sets the factory for creating prompt templates.
	/// </summary>
	public IPromptTemplateFactory? PromptTemplateFactory { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the temporary directory should be deleted after use.
	/// </summary>
	public bool DeleteTemporaryDirectory { get; set; } = true;

	/// <summary>
	/// Gets or sets the authentication configuration for accessing the Git repository.
	/// </summary>
	public GitPromptRepositoryAuthenticationConfiguration? Authentication { get; set; }

	/// <summary>
	/// Gets or sets the interval at which the system checks for updates.
	/// </summary>
	public TimeSpan UpdateCheckInterval { get; set; } = TimeSpan.FromMinutes(5);

	/// <summary>
	/// Gets the path to the local repository
	/// </summary>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException"></exception>
	public string GetLocalRepositoryPath()
	{
		if (string.IsNullOrEmpty(RepositoryUrl))
			throw new InvalidOperationException("Repository URL must be provided.");

		var rootPath = string.IsNullOrEmpty(TemporaryDirectory) ? System.IO.Path.Combine(System.IO.Path.GetTempPath(), "GitPrompts") : TemporaryDirectory;

		return System.IO.Path.Combine(rootPath, GetMd5(RepositoryUrl));
	}

	/// <summary>
	/// Validates the configuration of the Git prompt repository.
	/// </summary>
	/// <exception cref="InvalidOperationException"></exception>
	public void Validate()
	{
		if (string.IsNullOrEmpty(RepositoryUrl))
			throw new InvalidOperationException("Repository URL must be provided.");

		if (string.IsNullOrEmpty(Branch))
			throw new InvalidOperationException("Branch must be provided.");
	}

	private static string GetMd5(string value)
	{
		var bytes = MD5.HashData(Encoding.UTF8.GetBytes(value));
		return Convert.ToHexString(bytes);
	}
}

/// <summary>
/// The Git prompt repository authentication configuration.
/// </summary>
public class GitPromptRepositoryAuthenticationConfiguration
{
	/// <summary>
	/// Gets or sets the username for authentication.
	/// </summary>
	public string? Username { get; set; }

	/// <summary>
	/// Gets or sets the password for authentication.
	/// </summary>
	public string? Password { get; set; }
}
