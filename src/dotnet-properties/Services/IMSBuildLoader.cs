using System.Reflection;
using System.Runtime.Loader;

namespace DotNet.Properties.Services
{
    internal interface IMSBuildLoader
    {
        bool TryResolveMSBuildAssembly(AssemblyLoadContext context, string assemblyName, out Assembly? assembly);
    }
}
