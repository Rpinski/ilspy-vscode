using ICSharpCode.ILSpyX;
using ILSpy.Backend.Application;
using ILSpy.Backend.Model;
using ILSpyX.Backend.Application;
using Microsoft.Extensions.Logging.Abstractions;

namespace ILSpyX.Backend.Tests;

public class AnalyzerTreeNodeProviderTests
{
    private static string AssemblyPath => Path.Combine(Path.GetDirectoryName(typeof(AnalyzerTreeNodeProviderTests).Assembly.Location) ?? "", "TestAssembly.dll");

    private static async Task<ILSpyXApplication> CreateTestApplication()
    {
        var application = new ILSpyXApplication(new NullLoggerFactory(), new ILSpyBackendSettings());
        application.DecompilerBackend.AddAssembly(AssemblyPath);
        await application.AssemblyList.AddAssembly(AssemblyPath);
        return application;
    }

    [Fact]
    public async Task MethodAnalyzers()
    {
        var application = await CreateTestApplication();
        var types = application.TreeNodeProviders.Namespace.GetChildren(
            new NodeMetadata(AssemblyPath, NodeType.Namespace, "TestAssembly", 0, 0));
        var typeNode = types.Where(node => node.Metadata?.Name == "SomeClass").First();
        var members = application.TreeNodeProviders.Type.GetChildren(typeNode.Metadata);
        var methodNode = members.First(node => node.Metadata?.Name?.StartsWith("ToString") ?? false);
        var analyzerNodes = application.TreeNodeProviders.AnalyzersRoot.GetChildren(methodNode.Metadata);
        Assert.Collection(analyzerNodes,
                node => {
                    Assert.Equal("Used by", node.DisplayName);
                    Assert.Equal("Used by", node.Description);
                    Assert.True(node.MayHaveChildren);
                    Assert.Equal(AssemblyPath, node.Metadata?.AssemblyPath);
                    Assert.Equal(NodeType.MethodUsedBy, node.Metadata?.Type);
                    Assert.Equal(methodNode.Metadata?.SymbolToken, node.Metadata?.SymbolToken);
                    Assert.Equal(methodNode.Metadata?.ParentSymbolToken, node.Metadata?.ParentSymbolToken);
                });

        var methodUsedByNodes = application.TreeNodeProviders.MethodUsedBy.GetChildren(analyzerNodes.ElementAt(0).Metadata);
        var callerStructTypeNode = types.Where(node => node.Metadata?.Name == "SomeStruct").First();
        Assert.Collection(methodUsedByNodes,
                node => {
                    Assert.Equal("SomeMethod() : string", node.Metadata?.Name);
                    Assert.Equal(NodeType.Method, node.Metadata?.Type);
                    Assert.Equal(callerStructTypeNode.Metadata?.SymbolToken, node.Metadata?.ParentSymbolToken);
                    Assert.Equal(SymbolModifiers.Public, node.SymbolModifiers);
                    Assert.False(node.MayHaveChildren);
                });
    }
}