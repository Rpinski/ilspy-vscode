using ICSharpCode.Decompiler.TypeSystem;
using ICSharpCode.ILSpyX.Abstractions;
using ICSharpCode.ILSpyX.Analyzers.Builtin;
using ILSpy.Backend.Application;
using ILSpy.Backend.Model;
using ILSpyX.Backend.TreeProviders.Analyzers;
using K4os.Compression.LZ4.Internal;
using System;
using System.Security.Cryptography;

namespace ILSpy.Backend.TreeProviders;

public class TreeNodeProviders
{
    private readonly ILSpyXApplication application;

    public ITreeNodeProvider ForNode(NodeMetadata? nodeMetadata) =>
        FromNodeType(nodeMetadata?.Type);

    public ITreeNodeProvider FromNodeType(NodeType? nodeType) => nodeType switch
    {
        null => AssemblyTreeRoot,
        NodeType.Assembly => Assembly,
        NodeType.ReferencesRoot => ReferencesRoot,
        NodeType.AssemblyReference => AssemblyReference,
        NodeType.Namespace => Namespace,
        var type when NodeTypeHelper.IsTypeNode(type.Value) => Type,
        var type when NodeTypeHelper.IsMemberNode(type.Value) => Member,
        NodeType.MethodUsedBy => MethodUsedBy,
        _ => Dummy
    };

    public ITreeNodeProvider FromSymbolKind(SymbolKind symbolKind) => symbolKind switch
    {
        SymbolKind.TypeDefinition => Type,
        SymbolKind.Field => Member,
        SymbolKind.Property => Member,
        SymbolKind.Indexer => Member,
        SymbolKind.Event => Member,
        SymbolKind.Method => Member,
        SymbolKind.Operator => Member,
        SymbolKind.Constructor => Member,
        SymbolKind.Destructor => Member,
        SymbolKind.Namespace => Namespace,
        _ => Dummy,
    };

    public TreeNodeProviders(ILSpyXApplication application)
    {
        this.application = application;

        Dummy = new DummyTreeNodeProvider();

        AssemblyTreeRoot = new AssemblyTreeRootNodesProvider(application);
        Assembly = new AssemblyNodeProvider(application);
        ReferencesRoot = new ReferencesRootNodeProvider(application);
        AssemblyReference = new AssemblyReferenceNodeProvider(application);
        Namespace = new NamespaceNodeProvider(application);
        Type = new TypeNodeProvider(application);
        Member = new MemberNodeProvider(application);

        /*
        analyzers = [
            new AttributeAppliedToAnalyzer(),
            new EventImplementedByAnalyzer(),
            new EventOverriddenByAnalyzer(),
            new AssignedByFieldAccessAnalyzer(),
            new ReadByFieldAccessAnalyzer(),
            new MemberImplementsInterfaceAnalyzer(),
            new MethodImplementedByAnalyzer(),
            new MethodOverriddenByAnalyzer(),
            new MethodUsedByAnalyzer(),
            new MethodUsesAnalyzer(),
            new MethodVirtualUsedByAnalyzer(),
            new PropertyImplementedByAnalyzer(),
            new PropertyOverriddenByAnalyzer(),
            new TypeExposedByAnalyzer(),
            new TypeExtensionMethodsAnalyzer(),
            new TypeInstantiatedByAnalyzer(),
            new TypeUsedByAnalyzer()
        ];
        */
        AnalyzersRoot = new AnalyzersRootNodesProvider(application);
        MethodUsedBy = new AnalyzerNodeProvider(application, new MethodUsedByAnalyzer(), NodeType.MethodUsedBy, "Used by");
    }

    public DummyTreeNodeProvider Dummy { get; }
    public AssemblyTreeRootNodesProvider AssemblyTreeRoot { get; }
    public AssemblyNodeProvider Assembly { get; }
    public ReferencesRootNodeProvider ReferencesRoot { get; }
    public AssemblyReferenceNodeProvider AssemblyReference { get; }
    public NamespaceNodeProvider Namespace { get; }
    public TypeNodeProvider Type { get; }
    public MemberNodeProvider Member { get; }
    public AnalyzersRootNodesProvider AnalyzersRoot { get; }
    public AnalyzerNodeProvider MethodUsedBy { get; }
}

