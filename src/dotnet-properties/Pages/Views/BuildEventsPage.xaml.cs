using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

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
