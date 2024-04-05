using ILSpy.Backend.Application;
using ILSpy.Backend.Decompiler;
using ILSpy.Backend.Model;
using ILSpyX.Backend.TreeProviders.Analyzers;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ILSpy.Backend.TreeProviders;

public class AnalyzersRootNodesProvider : ITreeNodeProvider
{
    private readonly ILSpyXApplication application;

    public AnalyzersRootNodesProvider(ILSpyXApplication application)
    {
        this.application = application;
    }

    public DecompileResult Decompile(NodeMetadata nodeMetadata, string outputLanguage)
    {
        return DecompileResult.Empty();
    }

    public IEnumerable<Node> GetChildren(NodeMetadata? nodeMetadata)
    {
        var treeNodeProviders = application.TreeNodeProviders;
        IEnumerable<AnalyzerNodeProvider> analyzerNodeProviders = [
            treeNodeProviders.MethodUsedBy
        ];
        foreach (var provider in analyzerNodeProviders)
        {
            var node = provider.CreateNode(nodeMetadata);
            if (node is not null)
            {
                yield return node;
            }
        }
    }
}

