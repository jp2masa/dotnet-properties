using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using DotNet.Properties.Pages.ViewModels;

namespace DotNet.Properties.Pages.Views
{
    internal sealed class ApplicationPage : ReactiveUserControl<ApplicationPageViewModel>
    {
        public ApplicationPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
