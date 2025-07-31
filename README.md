# codeessentials.SemanticKernel.PromptTemplates.Git

[![NuGet](https://img.shields.io/nuget/v/codeessentials.SemanticKernel.PromptTemplates.Git.svg)](https://nuget.org/packages/codeessentials.SemanticKernel.PromptTemplates.Git/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![CI](https://github.com/twenzel/codeessentials.SemanticKernel.PromptTemplates.GitRepository/actions/workflows/build.yml/badge.svg)](https://github.com/twenzel/codeessentials.SemanticKernel.PromptTemplates.GitRepository/actions/workflows/build.yml)

[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=twenzel_codeessentials.SemanticKernel.PromptTemplates.GitRepository&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=twenzel_codeessentials.SemanticKernel.PromptTemplates.GitRepository)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=twenzel_codeessentials.SemanticKernel.PromptTemplates.GitRepository&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=twenzel_codeessentials.SemanticKernel.PromptTemplates.GitRepository)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=twenzel_codeessentials.SemanticKernel.PromptTemplates.GitRepository&metric=security_rating)](https://sonarcloud.io/dashboard?id=twenzel_codeessentials.SemanticKernel.PromptTemplates.GitRepository)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=twenzel_codeessentials.SemanticKernel.PromptTemplates.GitRepository&metric=bugs)](https://sonarcloud.io/dashboard?id=twenzel_codeessentials.SemanticKernel.PromptTemplates.GitRepository)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=twenzel_codeessentials.SemanticKernel.PromptTemplates.GitRepository&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=twenzel_codeessentials.SemanticKernel.PromptTemplates.GitRepository)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=twenzel_codeessentials.SemanticKernel.PromptTemplates.GitRepository&metric=coverage)](https://sonarcloud.io/dashboard?id=twenzel_codeessentials.SemanticKernel.PromptTemplates.GitRepository)


Microsoft Semantic Kernel extension to retrieve prompt templates from a git repository

## Installation

Add the NuGet package [codeessentials.SemanticKernel.PromptTemplates.Git](https://nuget.org/packagescodeessentials.SemanticKernel.PromptTemplates.Git/) to your project.

> &gt; dotnet add package codeessentials.SemanticKernel.PromptTemplates.Git

## Usage

Register the required services.

```csharp
services.AddGitPromptTemplates(config =>
{
	configuration.GetSection("GitPrompts").Bind(config);

	// Set the prompt template factory to use default and Handlebars templates
	config.PromptTemplateFactory = new AggregatorPromptTemplateFactory(
	new KernelPromptTemplateFactory(),
	new HandlebarsPromptTemplateFactory());
});

// or 
services.AddKernel()
    .AddGitPromptTemplates(config =>
    {
    	configuration.GetSection("GitPrompts").Bind(config);
    
    	// Set the prompt template factory to use default and Handlebars templates
    	config.PromptTemplateFactory = new AggregatorPromptTemplateFactory(
    	new KernelPromptTemplateFactory(),
    	new HandlebarsPromptTemplateFactory());
    });
```

Retrieve a prompt template from the git repository.
```csharp
// Get as KernelFunction
var function = _kernel.GetFunctionFromGitPrompt("UserChatPrompt");

var response = await _kernel.InvokeAsync(function, new KernelArguments
{
	{ "user", "John Doe" },
	{ "message", "Hello, how are you?" }
});

// Get as PromptTemplateConfig
var template = _kernel.GetPromptTemplateFromGitPrompt("UserChatPrompt");
```

## Configuration
The configuration is done via the `GitPromptTemplatesConfiguration` class.

Use the `DeleteTemporaryDirectory` property to delete the temporary directory (used to clone the repository) after the prompts have been loaded.
Setting this to `false` will keep the temporary directory and the cloned repository on disk, which improves the performance of loading the prompts, but requires more disk space.

Use the `UpdateCheckInterval` property to set the interval in seconds to check for updates to the git repository.
