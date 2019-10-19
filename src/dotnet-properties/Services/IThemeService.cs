using System.Collections.Generic;

namespace DotNet.Properties.Services
{
    internal interface IThemeService
    {
        ITheme CurrentTheme { get; set; }
        IReadOnlyCollection<ITheme> AvailableThemes { get; }
    }
}
