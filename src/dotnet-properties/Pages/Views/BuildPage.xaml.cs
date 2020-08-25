using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using DotNet.Properties.Pages.ViewModels;

namespace DotNet.Properties.Pages.Views
{
    internal sealed class BuildPage : ReactiveUserControl<BuildPageViewModel>
    {
        public BuildPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
