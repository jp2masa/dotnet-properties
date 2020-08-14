using System.ComponentModel;
using System.Windows.Input;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using DotNet.Properties.ViewModels;

namespace DotNet.Properties.Views
{
    internal class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public static readonly StyledProperty<ICommand> ClosingCommandProperty =
            AvaloniaProperty.Register<Window, ICommand>(nameof(ClosingCommand));

        public ICommand ClosingCommand
        {
            get => GetValue(ClosingCommandProperty);
            set => SetValue(ClosingCommandProperty, value);
        }

        public MainWindow()
        {
            InitializeComponent();
#if AVALONIA_DIAGNOSTICS
            this.AttachDevTools();
#endif

            Closing += HandleClosingCommand;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void HandleClosingCommand(object? sender, CancelEventArgs e)
        {
            if (ClosingCommand != null)
            {
                if (!ClosingCommand.CanExecute(e))
                {
                    e.Cancel = false;
                }
                else
                {
                    ClosingCommand.Execute(e);
                }
            }
        }
    }
}
