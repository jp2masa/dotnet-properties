// Code copied from Buildalyzer (https://github.com/daveaglick/Buildalyzer)

using System;
using System.Collections.Generic;

namespace DotNet.Properties
{
    internal sealed class BuildEnvironment : IDisposable
    {
        private readonly string? _oldMsBuildExtensionsPath;
        private readonly string? _oldMsBuildSdksPath;

        public BuildEnvironment(IReadOnlyDictionary<string, string> globalProperties)
        {
            if (globalProperties.TryGetValue(MSBuildProperties.MSBuildExtensionsPath, out var msBuildExtensionsPath))
            {
                _oldMsBuildExtensionsPath = Environment.GetEnvironmentVariable(MSBuildProperties.MSBuildExtensionsPath);
                Environment.SetEnvironmentVariable(MSBuildProperties.MSBuildExtensionsPath, msBuildExtensionsPath);
            }
            if (globalProperties.TryGetValue(MSBuildProperties.MSBuildSDKsPath, out var msBuildSDKsPath))
            {
                _oldMsBuildSdksPath = Environment.GetEnvironmentVariable(MSBuildProperties.MSBuildSDKsPath);
                Environment.SetEnvironmentVariable(MSBuildProperties.MSBuildSDKsPath, msBuildSDKsPath);
            }
        }

        public void Dispose()
        {
            if (_oldMsBuildExtensionsPath != null)
            {
                Environment.SetEnvironmentVariable(MSBuildProperties.MSBuildExtensionsPath, _oldMsBuildExtensionsPath);
            }
            if (_oldMsBuildSdksPath != null)
            {
                Environment.SetEnvironmentVariable(MSBuildProperties.MSBuildSDKsPath, _oldMsBuildSdksPath);
            }
        }
    }
}
