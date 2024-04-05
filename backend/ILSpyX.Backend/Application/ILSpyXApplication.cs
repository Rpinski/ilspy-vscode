using ICSharpCode.ILSpyX;
using ILSpy.Backend.Decompiler;
using ILSpy.Backend.TreeProviders;
using ILSpyX.Backend.Application;
using Microsoft.Extensions.Logging;

namespace ILSpy.Backend.Application;

public class ILSpyXApplication
{
    public ILSpyXApplication(ILoggerFactory loggerFactory, ILSpyBackendSettings ilspyBackendSettings)
    {
        AssemblyList = new SingleThreadAssemblyList(new AssemblyListManager(new DummySettingsProvider()));
        DecompilerBackend = new(loggerFactory, ilspyBackendSettings);
        TreeNodeProviders = new(this);
    }

    public SingleThreadAssemblyList AssemblyList { get; }
    public DecompilerBackend DecompilerBackend { get; }
    public TreeNodeProviders TreeNodeProviders { get; }
}

