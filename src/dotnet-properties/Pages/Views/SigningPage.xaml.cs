using Avalonia;
using Avalonia.Markup.Xaml;

using DotNet.Properties.Pages.ViewModels;

namespace DotNet.Properties.Pages.Views
{
    internal class SigningPage : ReactiveUserControl<SigningPageViewModel>
    {
        public SigningPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
