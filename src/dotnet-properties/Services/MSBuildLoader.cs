using System;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace DotNet.Properties.Services
{
    internal sealed class MSBuildLoader : IMSBuildLoader
    {
        private static readonly ImmutableHashSet<string> MSBuildAssemblies =
            ImmutableHashSet.Create(
                StringComparer.OrdinalIgnoreCase,
                "Microsoft.Build",
                "Microsoft.Build.Framework",
                "Microsoft.Build.Tasks.Core",
                "Microsoft.Build.Utilities.Core");

        private readonly DotNetSdkPaths _dotnetSdkPaths;

        public MSBuildLoader(DotNetSdkPaths dotnetSdkPaths)
        {
            _dotnetSdkPaths = dotnetSdkPaths;
        }

        public bool TryResolveMSBuildAssembly(AssemblyLoadContext context, string assemblyName, out Assembly? assembly)
        {
            if (MSBuildAssemblies.Contains(assemblyName))
            {
                var assemblyPath = Path.Combine(_dotnetSdkPaths.ToolsPath, $"{assemblyName}.dll");

                if (File.Exists(assemblyPath))
                {
                    try
                    {
                        assembly = context.LoadFromAssemblyPath(assemblyPath);
                        return true;
                    }
                    catch (FileLoadException)
                    {
                    }
                    catch (BadImageFormatException)
                    {
                    }
                }
            }

            assembly = null;
            return false;
        }
    }
}
