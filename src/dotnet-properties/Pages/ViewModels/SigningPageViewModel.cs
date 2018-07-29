using System;
using System.Collections.Generic;
using System.Windows.Input;

using Avalonia.Controls;

using DotNet.Properties.Services;

namespace DotNet.Properties.Pages.ViewModels
{
    internal class SigningPageViewModel : PropertyPageViewModel
    {
        private static class Property
        {
            public const string SignAssembly = nameof(SignAssembly);
            public const string AssemblyOriginatorKeyFile = nameof(AssemblyOriginatorKeyFile);
            public const string DelaySign = nameof(DelaySign);
        }

        public SigningPageViewModel(
            IPropertyManager propertyManager,
            IOpenFileDialogService openFileDialogService)
            : base(propertyManager)
        {
            OpenKeyFileCommand = new OpenKeyFileCommand(this, openFileDialogService);
        }

        public bool SignAssembly
        {
            get => GetBooleanProperty(Property.SignAssembly);
            set => SetBooleanProperty(Property.SignAssembly, value);
        }

        public string AssemblyOriginatorKeyFile
        {
            get => GetStringProperty(Property.AssemblyOriginatorKeyFile);
            set => SetStringProperty(Property.AssemblyOriginatorKeyFile, value);
        }

        public ICommand OpenKeyFileCommand { get; }

        public bool DelaySign
        {
            get => GetBooleanProperty(Property.DelaySign);
            set => SetBooleanProperty(Property.DelaySign, value);
        }
    }

    internal class OpenKeyFileCommand : ICommand
    {
#pragma warning disable CS0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067

        private readonly SigningPageViewModel _viewModel;
        private readonly IOpenFileDialogService _openFileDialogService;

        public OpenKeyFileCommand(
            SigningPageViewModel viewModel,
            IOpenFileDialogService openFileDialogService)
        {
            _viewModel = viewModel;
            _openFileDialogService = openFileDialogService;
        }

        public bool CanExecute(object parameter) => true;

        public async void Execute(object parameter)
        {
            var openFileDialog = new OpenFileDialog()
            {
                AllowMultiple = false,
                Filters = new List<FileDialogFilter>()
                {
                    new FileDialogFilter() { Name = "Key Files", Extensions = new List<string>() { "snk", "pfx" } }
                },
                Title = String.Empty
            };

            var paths = await _openFileDialogService.ShowDialogAsync(openFileDialog).ConfigureAwait(false);

            if (paths != null && paths.Count > 0)
            {
                _viewModel.AssemblyOriginatorKeyFile = paths[0];
            }
        }
    }
}
