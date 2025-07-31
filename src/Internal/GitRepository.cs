using LibGit2Sharp;

namespace codeessentials.SemanticKernel.PromptTemplates.Git.Internal;

internal class GitRepository : IGitRepository
{
	private readonly GitPromptRepositoryConfiguration _configuration;

	public GitRepository(GitPromptRepositoryConfiguration configuration)
	{
		_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
	}

	public string Clone(string targetDirectory)
	{
		var options = new CloneOptions { BranchName = _configuration.Branch };

		if (_configuration.Authentication is not null)
		{
			var credentials = new UsernamePasswordCredentials
			{
				Username = _configuration.Authentication.Username,
				Password = _configuration.Authentication.Password
			};
			options.FetchOptions.CredentialsProvider = (_, _, _) => credentials;
		}

		return Repository.Clone(_configuration.RepositoryUrl, targetDirectory, options);
	}

	public string? GetCurrentCommit()
	{
		var tempPath = _configuration.GetLocalRepositoryPath();

		try
		{
			using var repo = new Repository(tempPath);

			return repo.Head.Tip.Sha;
		}
		catch (RepositoryNotFoundException)
		{
			return null;
		}
	}

	public bool UpdateRepository()
	{
		var tempPath = _configuration.GetLocalRepositoryPath();
		using var repo = new Repository(tempPath);

		var options = new PullOptions
		{
			FetchOptions = new FetchOptions()
		};

		if (_configuration.Authentication is not null)
		{
			var credentials = new UsernamePasswordCredentials
			{
				Username = _configuration.Authentication.Username,
				Password = _configuration.Authentication.Password
			};
			options.FetchOptions.CredentialsProvider = (_, _, _) => credentials;
		}

		// User information to create a merge commit
		var signature = new LibGit2Sharp.Signature(
			new Identity("MERGE_USER_NAME", "MERGE_USER_EMAIL"), DateTimeOffset.Now);

		// Pull
		var mergeResult = Commands.Pull(repo, signature, options);
		return mergeResult.Status != MergeStatus.UpToDate;
	}
}
