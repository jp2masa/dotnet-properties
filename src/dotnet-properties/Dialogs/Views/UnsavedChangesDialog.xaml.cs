using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DotNet.Properties.Dialogs.Views
{
    internal class UnsavedChangesDialog : Window
    {
        public UnsavedChangesDialog()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
