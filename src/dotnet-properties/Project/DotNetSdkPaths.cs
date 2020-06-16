using System.IO;

namespace DotNet.Properties
{
    internal sealed class DotNetSdkPaths
    {
        public DotNetSdkPaths(string dotnetSdkPath)
        {
            ToolsPath = dotnetSdkPath;
            ExtensionsPath = dotnetSdkPath;
            SdksPath = Path.Combine(dotnetSdkPath, "Sdks");
            RoslynTargetsPath = Path.Combine(dotnetSdkPath, "Roslyn");
        }

        public string ToolsPath { get; }

        public string ExtensionsPath { get; }

        public string SdksPath { get; }

        public string RoslynTargetsPath { get; }
    }
}
