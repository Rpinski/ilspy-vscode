using ILSpyX.Backend;
using ILSpyX.Backend.MCP;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole(consoleLogOptions => {
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<AssemblyListTool>()
    .WithTools<DecompileTool>();

builder.Services
    .AddILSpyXServices()
    .AddILSpyXTreeNodeProviders();

await builder.Build().RunAsync();