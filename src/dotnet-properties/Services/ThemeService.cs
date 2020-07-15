using System.Collections.Generic;

using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;

namespace DotNet.Properties.Services
{
    internal sealed class ThemeService : IThemeService
    {
        private static readonly StyleInclude Styles = Style("avares://dotnet-properties/Styles/Styles.xaml");

        private static readonly StyleInclude AvaloniaDefault = Style("avares://Avalonia.Themes.Default/DefaultTheme.xaml");

        private static readonly StyleInclude AvaloniaDefaultBaseLight = AvaloniaDefaultAccent("BaseLight");
        private static readonly StyleInclude AvaloniaDefaultBaseDark = AvaloniaDefaultAccent("BaseDark");

        private static readonly StyleInclude AvaloniaFluentLight = AvaloniaFluentAccent("FluentLight");
        private static readonly StyleInclude AvaloniaFluentDark = AvaloniaFluentAccent("FluentDark");

        private static readonly StyleInclude BaseLightBlue = Style("avares://dotnet-properties/Styles/Accents/BaseLightBlue.xaml");

        private static readonly StyleInclude Citrus = CitrusTheme("Citrus");
        private static readonly StyleInclude CitrusSea = CitrusTheme("Sea");
        private static readonly StyleInclude CitrusRust = CitrusTheme("Rust");
        private static readonly StyleInclude CitrusCandy = CitrusTheme("Candy");
        private static readonly StyleInclude CitrusMagma = CitrusTheme("Magma");

        private static readonly Theme[] Themes = new Theme[]
        {
            new Theme("Light", new Styles() { AvaloniaDefault, AvaloniaDefaultBaseLight, Styles }),
            new Theme("Light Blue", new Styles() { AvaloniaDefault, BaseLightBlue, Styles }),
            new Theme("Dark", new Styles() { AvaloniaDefault, AvaloniaDefaultBaseDark, Styles }),
            new Theme("Fluent Light", new Styles() { AvaloniaFluentLight, Styles }),
            new Theme("Fluent Dark", new Styles() { AvaloniaFluentDark, Styles }),
            new Theme("Citrus", new Styles() { Citrus, Styles }),
            new Theme("Sea (Citrus)", new Styles() { CitrusSea, Styles }),
            new Theme("Rust (Citrus)", new Styles() { CitrusRust, Styles }),
            new Theme("Candy (Citrus)", new Styles() { CitrusCandy, Styles }),
            new Theme("Magma (Citrus)", new Styles() { CitrusMagma, Styles })
        };

        private readonly Application _app;

        private ITheme _currentTheme;

        public ThemeService(Application app)
        {
            _app = app;

            AvailableThemes = Themes;

            _currentTheme = Themes[0];
            _app.Styles.Insert(0, _currentTheme.Style);
        }

        public ITheme CurrentTheme
        {
            get => _currentTheme;
            set
            {
                _app.Styles.Remove(_currentTheme.Style);
                _currentTheme = value;
                _app.Styles.Insert(0, _currentTheme.Style);
            }
        }

        public IReadOnlyCollection<ITheme> AvailableThemes { get; }

        private static StyleInclude AvaloniaDefaultAccent(string name) => Style($"avares://Avalonia.Themes.Default/Accents/{name}.xaml");

        private static StyleInclude AvaloniaFluentAccent(string name) => Style($"avares://Avalonia.Themes.Fluent/Accents/{name}.xaml");

        private static StyleInclude CitrusTheme(string name) => Style($"avares://Citrus.Avalonia/{name}.xaml");

        private static StyleInclude Style(string source) => AvaloniaXamlLoader.Parse<StyleInclude>(
            $@"<StyleInclude xmlns=""https://github.com/avaloniaui"" Source=""{source}"" />");
    }
}
