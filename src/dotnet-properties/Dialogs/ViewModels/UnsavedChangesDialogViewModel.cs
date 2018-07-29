using System;
using System.Windows.Input;
using Avalonia.Controls;
using DotNet.Properties.Dialogs.Models;

namespace DotNet.Properties.Dialogs.ViewModels
{
    internal class UnsavedChangesDialogViewModel
    {
        public ICommand YesCommand { get; }
        public ICommand NoCommand { get; }
        public ICommand CancelCommand { get; }

        public UnsavedChangesDialogResult DialogResult { get; set; }

        public UnsavedChangesDialogViewModel()
        {
            YesCommand = new CloseCommand(this, UnsavedChangesDialogResult.Yes);
            NoCommand = new CloseCommand(this, UnsavedChangesDialogResult.No);
            CancelCommand = new CloseCommand(this, UnsavedChangesDialogResult.Cancel);

            DialogResult = UnsavedChangesDialogResult.Cancel;
        }

        private class CloseCommand : ICommand
        {
#pragma warning disable CS0067
            public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067

            private readonly UnsavedChangesDialogViewModel _viewModel;
            private readonly UnsavedChangesDialogResult _dialogResult;

            public CloseCommand(
                UnsavedChangesDialogViewModel viewModel,
                UnsavedChangesDialogResult dialogResult)
            {
                _viewModel = viewModel;
                _dialogResult = dialogResult;
            }

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter)
            {
                if (!(parameter is Window window))
                {
                    throw new InvalidOperationException("Internal error!");
                }

                _viewModel.DialogResult = _dialogResult;
                window.Close();
            }
        }
    }
}
