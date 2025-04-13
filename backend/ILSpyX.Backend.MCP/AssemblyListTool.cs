using ILSpyX.Backend.Application;
using ILSpyX.Backend.Decompiler;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace ILSpyX.Backend.MCP;

[McpServerToolType]
[SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods")]
public sealed class AssemblyListTool(DecompilerBackend decompilerBackend)
{
    [McpServerTool, Description("Adds a .NET assembly file to asssembly list.")]
    public async Task<string> AddAssembly([Description("File and path of assembly file to add")] string assemblyPath)
    {
        return (await decompilerBackend.AddAssemblyAsync(assemblyPath)) is not null
            ? "Adding assembly was successful"
            : "Adding assembly has failed";
    }

    [McpServerTool, Description("Removes a .NET assembly file from assembly list.")]
    public async Task<string> RemoveAssembly(
        [Description("File and path of assembyl file to remove")]
        string assemblyPath)
    {
        return (await decompilerBackend.RemoveAssemblyAsync(assemblyPath))
            ? "Removing assembly was successful"
            : "Removing assembly has failed";
    }

    [McpServerTool, Description("Get a list of all .NET assemblies in assembly list.")]
    public async Task<string> ListAssemblies()
    {
        return JsonSerializer.Serialize(
            (await decompilerBackend.GetLoadedAssembliesAsync()).Select(assembly => assembly.FilePath));
    }
}