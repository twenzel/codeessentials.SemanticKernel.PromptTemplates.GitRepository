using Microsoft.SemanticKernel;

namespace codeessentials.SemanticKernel.PromptTemplates.Git;

/// <summary>
/// Interface for updating Git prompts.
/// </summary>
public interface IGitPromptHandler
{
    /// <summary>
    /// Updates the Git prompts.
    /// </summary>
    Task UpdatePromptsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates and returns a new instance of a <see cref="KernelPlugin"/> associated with the specified <see cref="Kernel"/>.
    /// </summary>
    /// <param name="kernel">The <see cref="Kernel"/> instance to associate with the created plugin. This parameter cannot be null.</param>
    /// <returns>A new <see cref="KernelPlugin"/> instance configured for the specified <see cref="Kernel"/>.</returns>
    KernelPlugin CreateKernelPlugin(Kernel kernel);
}
