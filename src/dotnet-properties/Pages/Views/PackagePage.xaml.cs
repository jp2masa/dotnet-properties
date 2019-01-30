using Avalonia;
using Avalonia.Markup.Xaml;

using DotNet.Properties.Pages.ViewModels;

namespace DotNet.Properties.Pages.Views
{
    internal class PackagePage : ReactiveUserControl<PackagePageViewModel>
    {
        public PackagePage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
