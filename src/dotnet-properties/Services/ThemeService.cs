using System.Collections.Generic;

using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;

namespace DotNet.Properties.Services
{
    internal sealed class ThemeService : IThemeService
    {
        private static readonly StyleInclude Avalonia = Style("avares://Avalonia.Themes.Default/DefaultTheme.xaml");

        private static readonly StyleInclude AvaloniaBaseLight = AvaloniaAccent("BaseLight");
        private static readonly StyleInclude AvaloniaBaseDark = AvaloniaAccent("BaseDark");

        private static readonly StyleInclude BaseLightBlue = Style("avares://dotnet-properties/Styles/Accents/BaseLightBlue.xaml");

        private static readonly StyleInclude Citrus = CitrusTheme("Citrus");
        private static readonly StyleInclude CitrusSea = CitrusTheme("Sea");
        private static readonly StyleInclude CitrusRust = CitrusTheme("Rust");
        private static readonly StyleInclude CitrusCandy = CitrusTheme("Candy");
        private static readonly StyleInclude CitrusMagma = CitrusTheme("Magma");

        private static readonly Theme[] Themes = new Theme[]
        {
            new Theme("Light", new Styles() { Avalonia, AvaloniaBaseLight }),
            new Theme("Light Blue", new Styles() { Avalonia, BaseLightBlue }),
            new Theme("Dark", new Styles() { Avalonia, AvaloniaBaseDark }),
            new Theme("Citrus", new Styles() { Citrus }),
            new Theme("Sea (Citrus)", new Styles() { CitrusSea }),
            new Theme("Rust (Citrus)", new Styles() { CitrusRust }),
            new Theme("Candy (Citrus)", new Styles() { CitrusCandy }),
            new Theme("Magma (Citrus)", new Styles() { CitrusMagma })
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

        private static StyleInclude AvaloniaAccent(string name) => Style($"avares://Avalonia.Themes.Default/Accents/{name}.xaml");

        private static StyleInclude CitrusTheme(string name) => Style($"avares://Citrus.Avalonia/{name}.xaml");

        private static StyleInclude Style(string source) => AvaloniaXamlLoader.Parse<StyleInclude>(
            $@"<StyleInclude xmlns=""https://github.com/avaloniaui"" Source=""{source}"" />");
    }
}
