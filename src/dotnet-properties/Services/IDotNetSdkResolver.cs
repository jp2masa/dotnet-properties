namespace DotNet.Properties.Services
{
    internal interface IDotNetSdkResolver
    {
        bool TryResolveSdkPath(string workingDirectory, out string path);
    }
}
