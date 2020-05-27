using System;

namespace DotNet.Properties.Pages.Models
{
    internal sealed class RunPostBuildEvent : IEquatable<RunPostBuildEvent>
    {
        public RunPostBuildEvent(string value, string displayName)
        {
            Value = value;
            DisplayName = displayName;
        }

        public string Value { get; }

        public string DisplayName { get; }

        public bool Equals(RunPostBuildEvent? other) => !(other is null) && DisplayName.Equals(other.DisplayName, StringComparison.Ordinal);

        public override bool Equals(object? obj) => obj is RunPostBuildEvent other && Equals(other);

        public override int GetHashCode() => DisplayName.GetHashCode(StringComparison.Ordinal);

        public override string ToString() => DisplayName;
    }
}
