using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace codeessentials.SemanticKernel.PromptTemplates.Git.Internal;

/// <summary>
/// Class containing all yaml files
/// </summary>
internal static class TemplateRepository
{
#if NET9_0_OR_GREATER
	private static readonly Lock s_lock = new();
#else
	private static readonly object s_lock = new();
#endif

	public static List<TemplateItem> Items { get; } = [];

	public static void UpdateItems(string path, ILogger logger)
	{
		path = Path.GetFullPath(path);

		if (!Directory.Exists(path))
			throw new DirectoryNotFoundException($"Directory '{path}' could not be found.");

		lock (s_lock)
		{
			Items.Clear();

			var templateFiles = Directory.GetFiles(path, "*.yaml").Concat(Directory.GetFiles(path, "*.yml"));

			foreach (var templateFile in templateFiles)
			{
				if (logger.IsEnabled(LogLevel.Trace))
					logger.LogTrace("Loading yaml template from {Templatefile}", templateFile);

				var templateContent = File.ReadAllText(templateFile);
				var template = KernelFunctionYaml.ToPromptTemplateConfig(templateContent);

				var functionName = template.Name ?? Path.GetFileName(templateFile);

				Items.Add(new TemplateItem { Template = template, FileName = templateFile, Name = functionName });
			}
		}
	}
}

internal class TemplateItem
{
	public required PromptTemplateConfig Template { get; set; }
	public required string FileName { get; set; }
	public required string Name { get; set; }
}