using System;

namespace DotNet.Properties.Pages.Models
{
    internal sealed class OutputType : IEquatable<OutputType>
    {
        public OutputType(string value, string displayName)
        {
            Value = value;
            DisplayName = displayName;
        }

        public string Value { get; }

        public string DisplayName { get; }

        public bool Equals(OutputType other) => DisplayName.Equals(other.DisplayName, StringComparison.Ordinal);

        public override bool Equals(object? obj) => obj is OutputType && Equals((OutputType)obj);

        public override int GetHashCode() => DisplayName.GetHashCode(StringComparison.Ordinal);

        public override string ToString() => DisplayName;
    }
}
