using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using DotNet.Properties.Dialogs.ViewModels;

namespace DotNet.Properties.Dialogs.Views
{
    internal class UnsavedChangesDialog : ReactiveWindow<UnsavedChangesDialogViewModel>
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
