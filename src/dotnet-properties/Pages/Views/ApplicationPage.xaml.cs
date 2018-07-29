using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DotNet.Properties.Pages.Views
{
    internal class ApplicationPage : UserControl
    {
        public ApplicationPage()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
