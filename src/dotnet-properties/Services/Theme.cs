using System.Collections.Generic;

using Avalonia.Styling;

namespace DotNet.Properties.Services
{
    internal sealed class Theme : ITheme
    {
        public Theme(string name, IStyle style)
        {
            Name = name;
            Style = style;
        }

        public string Name { get; }

        public IStyle Style { get; }

        public override string ToString() => Name;
    }
}
