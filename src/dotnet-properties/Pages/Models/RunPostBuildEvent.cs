using System;

namespace DotNet.Properties.Pages.Models
{
    internal class RunPostBuildEvent : IEquatable<RunPostBuildEvent>
    {
        public string Value { get; }
        public string DisplayName { get; }

        public RunPostBuildEvent(string value, string displayName)
        {
            Value = value;
            DisplayName = displayName;
        }

        public bool Equals(RunPostBuildEvent other) => DisplayName.Equals(other.DisplayName, StringComparison.Ordinal);

        public override bool Equals(object obj) => obj is RunPostBuildEvent && Equals((RunPostBuildEvent)obj);

        public override int GetHashCode() => DisplayName.GetHashCode(StringComparison.Ordinal);

        public override string ToString() => DisplayName;
    }
}
