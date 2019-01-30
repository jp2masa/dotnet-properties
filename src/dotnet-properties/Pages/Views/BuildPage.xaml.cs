using Avalonia;
using Avalonia.Markup.Xaml;

using DotNet.Properties.Pages.ViewModels;

namespace DotNet.Properties.Pages.Views
{
    internal class BuildPage : ReactiveUserControl<BuildPageViewModel>
    {
        public BuildPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
