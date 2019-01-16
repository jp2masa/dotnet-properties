using System;
using System.Threading;
using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Threading;

namespace DotNet.Properties.Services
{
    internal class DialogService<TView> : IDialogService where TView : Window
    {
        private readonly Func<TView> _viewFactory;
        private readonly Window _owner;

        public DialogService(
            Func<TView> viewFactory,
            Window owner = null)
        {
            _viewFactory = viewFactory;
            _owner = owner;
        }

        public void Show(object viewModel)
        {
            var view = _viewFactory();

            view.DataContext = viewModel;
            view.Owner = _owner;

            if (_owner != null)
            {
                _owner.IsEnabled = false;
            }

            var source = new CancellationTokenSource();
            view.ShowDialog(_owner).ContinueWith(t => source.Cancel(), TaskScheduler.FromCurrentSynchronizationContext());

            Dispatcher.UIThread.MainLoop(source.Token);

            if (_owner != null)
            {
                _owner.IsEnabled = true;
            }
        }
    }
}
