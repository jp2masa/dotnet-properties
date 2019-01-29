using System.Collections.Generic;

using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;

namespace DotNet.Properties.Services
{
    internal sealed class ThemeService : IThemeService
    {
        private static readonly Theme[] Themes = new Theme[]
        {
            new Theme(
                "Light",
                AvaloniaXamlLoader.Parse<StyleInclude>(
                    @"<StyleInclude xmlns='https://github.com/avaloniaui' Source='avares://Avalonia.Themes.Default/Accents/BaseLight.xaml'/>")),
            new Theme(
                "Light Blue",
                AvaloniaXamlLoader.Parse<StyleInclude>(
                    @"<StyleInclude xmlns='https://github.com/avaloniaui' Source='avares://dotnet-properties/Styles/Accents/BaseLightBlue.xaml'/>")),
            new Theme(
                "Dark",
                AvaloniaXamlLoader.Parse<StyleInclude>(
                    @"<StyleInclude xmlns='https://github.com/avaloniaui' Source='avares://Avalonia.Themes.Default/Accents/BaseDark.xaml'/>"))
        };

        private readonly Application _app;

        private ITheme _currentTheme;

        public ThemeService(Application app)
        {
            _app = app;

            AvailableThemes = Themes;

            _currentTheme = Themes[0];
            _app.Styles.Add(_currentTheme.Style);
        }

        public ITheme CurrentTheme
        {
            get => _currentTheme;
            set
            {
                _app.Styles.Remove(_currentTheme.Style);
                _currentTheme = value;
                _app.Styles.Add(_currentTheme.Style);
            }
        }

        public IReadOnlyCollection<ITheme> AvailableThemes { get; }
    }
}
