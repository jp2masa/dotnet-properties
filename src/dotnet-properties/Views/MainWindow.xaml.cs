using System.ComponentModel;
using System.Windows.Input;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.Styling;

using DotNet.Properties.ViewModels;

namespace DotNet.Properties.Views
{
    internal class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        private static readonly Style DummyStyle = new Style();

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
#if DEBUG
            this.AttachDevTools();
#endif

            Closing += HandleClosingCommand;
        }

        // the workaround in App isn't enough, as the window is only added to Application.Windows on Show
        public override void Show()
        {
            base.Show();

            Styles.Add(DummyStyle);
            Styles.Remove(DummyStyle);
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
