using System;
using System.Threading;
using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Threading;

namespace DotNet.Properties.Services
{
    internal class DialogService<TView, TViewModel> : IDialogService<TViewModel> where TView : Window
    {
        private readonly Func<TView> _viewFactory;
        private readonly Window _owner;

        public DialogService(
            Func<TView> viewFactory,
            Window owner)
        {
            _viewFactory = viewFactory;
            _owner = owner;
        }

        public void Show(TViewModel viewModel)
        {
            var view = _viewFactory();
            view.DataContext = viewModel;

            using (var source = new CancellationTokenSource())
            {
                view.ShowDialog(_owner).ContinueWith(t => source.Cancel(), TaskScheduler.FromCurrentSynchronizationContext());
                Dispatcher.UIThread.MainLoop(source.Token);
            }
        }
    }
}
