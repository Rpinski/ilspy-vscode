using ICSharpCode.Decompiler.TypeSystem;
using ICSharpCode.ILSpyX.Analyzers;
using ILSpy.Backend.Application;
using ILSpy.Backend.Decompiler;
using ILSpy.Backend.Model;
using ILSpy.Backend.TreeProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace ILSpyX.Backend.TreeProviders.Analyzers;

public class AnalyzerNodeProvider : ITreeNodeProvider
{
    private readonly ILSpyXApplication application;
    private readonly IAnalyzer analyzer;
    private readonly NodeType nodeType;
    private readonly string displayName;

    public AnalyzerNodeProvider(ILSpyXApplication application, IAnalyzer analyzer, NodeType nodeType, string displayName)
    {
        this.application = application;
        this.analyzer = analyzer;
        this.nodeType = nodeType;
        this.displayName = displayName;
    }

    public DecompileResult Decompile(NodeMetadata nodeMetadata, string outputLanguage)
    {
        return DecompileResult.Empty();
    }

    public Node? CreateNode(NodeMetadata? nodeMetadata)
    {
        if (nodeMetadata is not null)
        {
            IEntity? nodeEntity = application.DecompilerBackend.GetEntityFromHandle(
                nodeMetadata.AssemblyPath, MetadataTokens.EntityHandle(nodeMetadata.SymbolToken));
            if (nodeEntity is not null && analyzer.Show(nodeEntity))
            {
                return new Node(
                    Metadata: nodeMetadata with { Type = nodeType },
                    DisplayName: displayName,
                    Description: displayName,
                    MayHaveChildren: true,
                    SymbolModifiers: SymbolModifiers.None
                );
            }
        }

        return null;
    }

    public IEnumerable<Node> GetChildren(NodeMetadata? nodeMetadata)
    {
        var assemblyList = application.AssemblyList.AssemblyList;
        if (nodeMetadata is not null && nodeMetadata.Type == nodeType && assemblyList is not null)
        {
            var context = new AnalyzerContext()
            {
                CancellationToken = new CancellationToken(),
                Language = new CSharpLanguage(),
                AssemblyList = assemblyList
            };

            var nodeEntity = application.DecompilerBackend.GetEntityFromHandle(
                nodeMetadata.AssemblyPath, MetadataTokens.EntityHandle(nodeMetadata.SymbolToken));
            if (nodeEntity is not null && analyzer.Show(nodeEntity))
            {
                var symbols = analyzer.Analyze(nodeEntity, context);
                if (symbols is not null)
                {
                    foreach (var symbol in symbols)
                    {
                        if (symbol is IEntity entity)
                        {
                            string nodeName = symbol is IMethod method
                                ? method.MethodToString(false, false, false)
                                : symbol.Name;
                            yield return new Node(
                                Metadata: new NodeMetadata(
                                    AssemblyPath: entity.Compilation.MainModule.MetadataFile?.FileName ?? "",
                                    Type: NodeTypeHelper.GetNodeTypeFromEntity(entity),
                                    Name: nodeName,
                                    SymbolToken: MetadataTokens.GetToken(entity.MetadataToken),
                                    ParentSymbolToken:
                                        entity.DeclaringTypeDefinition?.MetadataToken is not null ?
                                        MetadataTokens.GetToken(entity.DeclaringTypeDefinition.MetadataToken) : 0),
                                DisplayName: nodeName,
                                Description: nodeName,
                                MayHaveChildren: false,
                                SymbolModifiers: NodeTypeHelper.GetSymbolModifiers(entity));
                        }
                    }
                }
            }
        }
    }
}

