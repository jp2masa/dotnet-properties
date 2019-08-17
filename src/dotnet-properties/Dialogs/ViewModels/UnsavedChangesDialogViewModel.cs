using System.Windows.Input;

using Avalonia.Controls;

using ReactiveUI;

using DotNet.Properties.Dialogs.Models;

namespace DotNet.Properties.Dialogs.ViewModels
{
    internal class UnsavedChangesDialogViewModel : ReactiveObject
    {
        private UnsavedChangesDialogResult _dialogResult = UnsavedChangesDialogResult.Cancel;

        public UnsavedChangesDialogViewModel()
        {
            YesCommand = ReactiveCommand.Create<Window>(window => Close(UnsavedChangesDialogResult.Yes, window));
            NoCommand = ReactiveCommand.Create<Window>(window => Close(UnsavedChangesDialogResult.No, window));
            CancelCommand = ReactiveCommand.Create<Window>(window => Close(UnsavedChangesDialogResult.Cancel, window));
        }

        public ICommand YesCommand { get; }

        public ICommand NoCommand { get; }

        public ICommand CancelCommand { get; }

        public UnsavedChangesDialogResult DialogResult
        {
            get => _dialogResult;
            set => this.RaiseAndSetIfChanged(ref _dialogResult, value);
        }

        private void Close(UnsavedChangesDialogResult dialogResult, Window window)
        {
            DialogResult = dialogResult;
            window.Close();
        }
    }
}
