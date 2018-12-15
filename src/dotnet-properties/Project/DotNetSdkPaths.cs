using System.IO;

namespace DotNet.Properties
{
    internal class DotNetSdkPaths
    {
        public string ToolsPath { get; }
        public string ExtensionsPath { get; }
        public string SdksPath { get; }
        public string RoslynTargetsPath { get; }

        public DotNetSdkPaths(string dotnetSdkPath)
        {
            ToolsPath = dotnetSdkPath;
            ExtensionsPath = dotnetSdkPath;
            SdksPath = Path.Combine(dotnetSdkPath, "Sdks");
            RoslynTargetsPath = Path.Combine(dotnetSdkPath, "Roslyn");
        }
    }
}
