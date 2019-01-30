using Avalonia;
using Avalonia.Markup.Xaml;

using DotNet.Properties.Pages.ViewModels;

namespace DotNet.Properties.Pages.Views
{
    internal class BuildEventsPage : ReactiveUserControl<BuildEventsPageViewModel>
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
