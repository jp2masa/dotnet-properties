using System;
using System.Collections.Generic;

using Avalonia;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;

namespace DotNet.Properties.Services
{
    internal sealed class ThemeService : IThemeService
    {
        private static readonly Uri StylesBase = new Uri("avares://dotnet-properties/Styles/");
        private static readonly Uri Accents = new Uri(StylesBase, "Accents/");

        private static readonly Uri AvaloniaDefaultAccents = new Uri("avares://Avalonia.Themes.Default/Accents/");
        private static readonly Uri AvaloniaFluentAccents = new Uri("avares://Avalonia.Themes.Fluent/Accents/");

        private static readonly Uri CitrusBase = new Uri("avares://Citrus.Avalonia/");

        private static readonly StyleInclude Styles = Style(StylesBase, "Styles.xaml");

        private static readonly StyleInclude AvaloniaDefault = Style("avares://Avalonia.Themes.Default/DefaultTheme.xaml");

        private static readonly StyleInclude AvaloniaDefaultBaseLight = Style(AvaloniaDefaultAccents, "BaseLight.xaml");
        private static readonly StyleInclude AvaloniaDefaultBaseDark = Style(AvaloniaDefaultAccents, "BaseDark.xaml");

        private static readonly StyleInclude AvaloniaFluentBase = Style(AvaloniaFluentAccents, "Base.xaml");
        private static readonly StyleInclude AvaloniaFluentBaseLight = Style(AvaloniaFluentAccents, "BaseLight.xaml");
        private static readonly StyleInclude AvaloniaFluentBaseDark = Style(AvaloniaFluentAccents, "BaseDark.xaml");

        private static readonly StyleInclude AvaloniaFluentLight = Style(AvaloniaFluentAccents, "FluentLight.xaml");
        private static readonly StyleInclude AvaloniaFluentDark = Style(AvaloniaFluentAccents, "FluentDark.xaml");

        private static readonly StyleInclude BaseLightBlue = Style(Accents, "BaseLightBlue.xaml");

        private static readonly StyleInclude Citrus = Style(CitrusBase, "Citrus.xaml");
        private static readonly StyleInclude CitrusSea = Style(CitrusBase, "Sea.xaml");
        private static readonly StyleInclude CitrusRust = Style(CitrusBase, "Rust.xaml");
        private static readonly StyleInclude CitrusCandy = Style(CitrusBase, "Candy.xaml");
        private static readonly StyleInclude CitrusMagma = Style(CitrusBase, "Magma.xaml");

        private static readonly StyleInclude DefaultThemeAccents = Style(Accents, "DefaultThemeAccents.xaml");
        private static readonly StyleInclude FluentThemeAccents = Style(Accents, "FluentThemeAccents.xaml");
        private static readonly StyleInclude CitrusThemeAccents = Style(Accents, "CitrusThemeAccents.xaml");

        private static readonly Theme[] Themes = new Theme[]
        {
            new Theme("Light", new Styles() { AvaloniaFluentBase, AvaloniaFluentBaseLight, AvaloniaDefault, AvaloniaDefaultBaseLight, DefaultThemeAccents, Styles }),
            new Theme("Light Blue", new Styles() { AvaloniaFluentBase, AvaloniaFluentBaseLight, AvaloniaDefault, BaseLightBlue, DefaultThemeAccents, Styles }),
            new Theme("Dark", new Styles() { AvaloniaFluentBase, AvaloniaFluentBaseDark, AvaloniaDefault, AvaloniaDefaultBaseDark, DefaultThemeAccents, Styles }),
            new Theme("Fluent Light", new Styles() { AvaloniaFluentLight, FluentThemeAccents, Styles }),
            new Theme("Fluent Dark", new Styles() { AvaloniaFluentDark, FluentThemeAccents, Styles }),
            new Theme("Citrus", new Styles() { Citrus, CitrusThemeAccents, Styles }),
            new Theme("Sea (Citrus)", new Styles() { CitrusSea, CitrusThemeAccents, Styles }),
            new Theme("Rust (Citrus)", new Styles() { CitrusRust, CitrusThemeAccents, Styles }),
            new Theme("Candy (Citrus)", new Styles() { CitrusCandy, CitrusThemeAccents, Styles }),
            new Theme("Magma (Citrus)", new Styles() { CitrusMagma, CitrusThemeAccents, Styles }),
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

        private static StyleInclude Style(string source) => Style(new Uri(source));

        private static StyleInclude Style(Uri source) => new StyleInclude(source) { Source = source };

        private static StyleInclude Style(Uri baseUri, string source) => new StyleInclude(baseUri) { Source = new Uri(source, UriKind.Relative) };
    }
}
