using System.Collections.Generic;
using System.Threading.Tasks;

using Avalonia.Controls;

namespace DotNet.Properties.Services
{
    internal interface IOpenFileDialogService
    {
        Task<IReadOnlyList<string>> ShowDialogAsync(OpenFileDialog openFileDialog);
    }
}
