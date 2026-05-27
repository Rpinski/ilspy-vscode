using ILSpyX.Backend.Model;

namespace ILSpyX.Backend.Decompiler;

public class DecompileResult
{
    public string? DecompiledCode { get; init; }
    public bool IsError { get; init; }
    public string? ErrorMessage { get; init; }
    public DecompiledOutputType OutputType { get; init; }

    private DecompileResult()
    {
    }

    public static DecompileResult Empty() => new()
    {
        DecompiledCode = null, IsError = false, OutputType = DecompiledOutputType.NoData
    };

    public static DecompileResult WithCode(string? decompiledCode, DecompiledOutputType outputType) => new()
    {
        DecompiledCode = decompiledCode, IsError = false, OutputType = outputType
    };

    public static DecompileResult WithError(string? errorMessage) => new()
    {
        IsError = true, ErrorMessage = errorMessage, OutputType = DecompiledOutputType.NoData
    };
}