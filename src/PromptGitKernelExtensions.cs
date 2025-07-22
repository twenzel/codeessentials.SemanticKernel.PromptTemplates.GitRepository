using codeessentials.SemanticKernel.PromptTemplates.Git;

namespace Microsoft.SemanticKernel;

/// <summary>
/// Class for extensions methods to define functions retrieveing YAML prompts from Git.
/// </summary>
public static class PromptGitKernelExtensions
{
	/// <summary>
	/// The name of the Git prompt plugin.
	/// </summary>
	public const string GIT_PROMPT_PLUGIN_NAME = "GitPromptPlugin";

	/// <summary>
	/// Returns a <see cref="KernelFunction"/> from the Git prompt plugin by its name.
	/// </summary>
	/// <param name="kernel"></param>
	/// <param name="name"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static KernelFunction GetFunctionFromGitPrompt(this Kernel kernel, string name)
	{
		var plugin = kernel.Plugins.FirstOrDefault(p => p.Name == GIT_PROMPT_PLUGIN_NAME);

		plugin ??= kernel.CreatePluginFromPromptGitYaml();

		var function = plugin.FirstOrDefault(f => f.Name == name);
		if (function != null)
			return function;

		throw new ArgumentException($"Function '{name}' not found in plugin '{GIT_PROMPT_PLUGIN_NAME}'.", nameof(name));
	}

	/// <summary>
	/// Creates a new <see cref="KernelPlugin"/> from the Git prompt YAML files.
	/// </summary>
	/// <param name="kernel"></param>
	/// <returns></returns>
	public static KernelPlugin CreatePluginFromPromptGitYaml(this Kernel kernel)
	{
		var handler = kernel.GetRequiredService<IGitPromptHandler>();

		return handler.CreateKernelPlugin(kernel);
	}
}
