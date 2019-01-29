using Avalonia.Styling;

namespace DotNet.Properties.Services
{
    internal interface ITheme
    {
        string Name { get; }
        IStyle Style { get; }
    }
}
