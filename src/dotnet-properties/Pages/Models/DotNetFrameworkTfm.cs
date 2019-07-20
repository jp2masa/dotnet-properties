using System;

namespace DotNet.Properties.Pages.Models
{
    internal sealed class DotNetFrameworkTfm
    {
        public DotNetFrameworkTfm(string shortName, Version version)
        {
            ShortName = shortName;
            Version = version;
        }

        public string ShortName { get; }

        public Version Version { get; }
    }
}
