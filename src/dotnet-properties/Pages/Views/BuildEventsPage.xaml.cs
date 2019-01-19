using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DotNet.Properties.Pages.Views
{
    internal class BuildEventsPage : UserControl
    {
        public BuildEventsPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
