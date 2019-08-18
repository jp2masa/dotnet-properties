using System;
using System.Collections.Generic;

using Avalonia.Styling;

namespace DotNet.Properties.Services
{
    internal sealed class Theme : ITheme, IEquatable<Theme>
    {
        public Theme(string name, IStyle style)
        {
            Name = name;
            Style = style;
        }

        public string Name { get; }

        public IStyle Style { get; }

        public bool Equals(Theme other) =>
            !(other is null) && Name == other.Name;

        public override int GetHashCode() =>
            Name.GetHashCode(StringComparison.Ordinal);

        public override bool Equals(object obj) =>
            obj is Theme theme && Equals(theme);

        public override string ToString() => Name;

        public static bool operator ==(Theme theme1, Theme theme2) =>
            EqualityComparer<Theme>.Default.Equals(theme1, theme2);

        public static bool operator !=(Theme theme1, Theme theme2) =>
            !(theme1 == theme2);
    }
}
