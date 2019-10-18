using System.Diagnostics.CodeAnalysis;

namespace DotNet.Properties.Services
{
    internal interface IDotNetSdkResolver
    {
        bool TryResolveSdkPath(string workingDirectory, [NotNullWhen(true)]out string? path);
    }
}
