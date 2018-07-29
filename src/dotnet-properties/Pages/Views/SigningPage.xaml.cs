using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DotNet.Properties.Pages.Views
{
    internal class SigningPage : UserControl
    {
        public SigningPage()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
