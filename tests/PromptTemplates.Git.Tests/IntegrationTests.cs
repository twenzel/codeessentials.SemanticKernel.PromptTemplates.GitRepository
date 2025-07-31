using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;
using Shouldly;

namespace PromptTemplates.Git.Tests;

public class IntegrationTests
{
	private Kernel _kernel;

	[SetUp]
	public void Setup()
	{
		var services = new ServiceCollection();
		services.AddLogging();
		services.AddKernel()
		.AddGitPromptTemplates(config =>
		{
			config.RepositoryUrl = "https://github.com/twenzel/codeessentials.SemanticKernel.PromptTemplates.GitRepository";
			config.Branch = "main";
			config.Path = "tests/PromptTemplates.Git.Tests/Prompts";
			config.DeleteTemporaryDirectory = false;
			config.LocalRepositoryPath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "../../../../../"));

			// Set the prompt template factory to use default and Handlebars templates
			config.PromptTemplateFactory = new AggregatorPromptTemplateFactory(
			new KernelPromptTemplateFactory(),
			new HandlebarsPromptTemplateFactory());
		});

		var serviceProvider = services.BuildServiceProvider();
		_kernel = serviceProvider.GetRequiredService<Kernel>();
	}

	[Test]
	public void GetTemplateAsFunction()
	{
		var function = _kernel.GetFunctionFromGitPrompt("UserChatPrompt");
		function.ShouldNotBeNull();
	}

	[Test]
	public void GetTemplateAsPrompt()
	{
		var template = _kernel.GetPromptTemplateFromGitPrompt("UserChatPrompt");
		template.ShouldNotBeNull();
	}
}
