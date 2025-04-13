using ILSpyX.Backend.Application;
using ILSpyX.Backend.Model;
using ILSpyX.Backend.Search;
using ILSpyX.Backend.TreeProviders;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;

namespace ILSpyX.Backend.MCP;

[McpServerToolType]
[SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods")]
public sealed class DecompileTool(TreeNodeProviders treeNodeProviders, SearchBackend searchBackend)
{
    [McpServerTool, Description("Gets the C# code of the given symbol")]
    public async Task<string> Decompile(string symbol)
    {
        var result = (await searchBackend.Search(symbol, CancellationToken.None)).FirstOrDefault();
        if (result?.Metadata is not null)
        {
            return treeNodeProviders.ForNode(result.Metadata)
                       .Decompile(result.Metadata, LanguageName.CSharpLatest).DecompiledCode ??
                   "Symbol could not be decompiled";
        }

        return "Symbol could not be found.";
    }
}