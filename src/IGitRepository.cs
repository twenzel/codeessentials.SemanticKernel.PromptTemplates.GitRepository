namespace codeessentials.SemanticKernel.PromptTemplates.Git;

/// <summary>
/// Abstract interface for a Git repository.
/// </summary>
public interface IGitRepository
{
	/// <summary>
	/// Clones the repository to the specified target directory.
	/// </summary>
	/// <param name="targetDirectory"></param>
	/// <returns></returns>
	string Clone(string targetDirectory);

	/// <summary>
	/// Gets the current commit hash of the repository.
	/// </summary>
	/// <returns></returns>
	string? GetCurrentCommit();

	/// <summary>
	/// Updates the local repository by pulling the latest changes from the remote repository.
	/// </summary>
	/// <returns>Returns true if anything was updated.</returns>
	bool UpdateRepository();
}
