using System.Collections.Generic;
using System.Threading.Tasks;

using Avalonia.Controls;

namespace DotNet.Properties.Services
{
    internal class OpenFileDialogService : IOpenFileDialogService
    {
        private readonly Window _window;

        public OpenFileDialogService(Window window)
        {
            _window = window;
        }

        public async Task<IReadOnlyList<string>> ShowDialogAsync(OpenFileDialog openFileDialog) =>
            await openFileDialog.ShowAsync(_window).ConfigureAwait(false);
    }
}
