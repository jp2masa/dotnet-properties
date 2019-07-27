using System;

using NuGet.Frameworks;

namespace DotNet.Properties.Pages.Models
{
    internal class TargetFramework : IEquatable<TargetFramework>
    {
        public TargetFramework(NuGetFramework framework, string displayName)
        {
            NuGetFramework = framework;
            DisplayName = displayName;
            ShortName = framework.GetShortFolderName();
        }

        public TargetFramework(string framework, string displayName)
            : this(NuGetFramework.Parse(framework), displayName)
        {
        }

        public NuGetFramework NuGetFramework { get; }

        public string DisplayName { get; }

        public string ShortName { get; }

        public override string ToString() => DisplayName;

        public bool Equals(TargetFramework other) => NuGetFramework.Equals(other.NuGetFramework);

        public override bool Equals(object obj) => obj is TargetFramework && Equals((TargetFramework)obj);

        public override int GetHashCode() => NuGetFramework.GetHashCode();
    }
}
