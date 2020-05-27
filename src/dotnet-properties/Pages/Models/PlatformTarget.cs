using System;

namespace DotNet.Properties.Pages.Models
{
    internal sealed class PlatformTarget : IEquatable<PlatformTarget>
    {
        public PlatformTarget(string value, string displayName)
        {
            Value = value;
            DisplayName = displayName;
        }

        public string Value { get; }

        public string DisplayName { get; }

        public bool Equals(PlatformTarget? other) => !(other is null) && DisplayName.Equals(other.DisplayName, StringComparison.Ordinal);

        public override bool Equals(object? obj) => obj is PlatformTarget other && Equals(other);

        public override int GetHashCode() => DisplayName.GetHashCode(StringComparison.Ordinal);

        public override string ToString() => DisplayName;
    }
}
