using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DotNet.Properties.Pages.Views
{
    internal class BuildPage : UserControl
    {
        public BuildPage()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
