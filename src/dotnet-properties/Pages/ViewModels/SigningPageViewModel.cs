using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using Avalonia.Controls;

using ReactiveUI;

using DotNet.Properties.Services;

namespace DotNet.Properties.Pages.ViewModels
{
    internal sealed class SigningPageViewModel : PropertyPageViewModel
    {
        private static class Property
        {
            public const string SignAssembly = nameof(SignAssembly);
            public const string AssemblyOriginatorKeyFile = nameof(AssemblyOriginatorKeyFile);
            public const string DelaySign = nameof(DelaySign);
        }

        private readonly IOpenFileDialogService _openFileDialogService;

        public SigningPageViewModel(
            IPropertyManager propertyManager,
            IOpenFileDialogService openFileDialogService)
            : base(propertyManager)
        {
            _openFileDialogService = openFileDialogService;

            OpenKeyFileCommand = ReactiveCommand.Create(OpenKeyFile);
        }

        public bool SignAssembly
        {
            get => GetBooleanProperty(Property.SignAssembly);
            set => SetBooleanProperty(Property.SignAssembly, value);
        }

        public string? AssemblyOriginatorKeyFile
        {
            get => GetStringProperty(Property.AssemblyOriginatorKeyFile);
            set => SetStringProperty(Property.AssemblyOriginatorKeyFile, PropertyManager.MakeRelativePath(value));
        }

        public ICommand OpenKeyFileCommand { get; }

        public bool DelaySign
        {
            get => GetBooleanProperty(Property.DelaySign);
            set => SetBooleanProperty(Property.DelaySign, value);
        }

        private async Task OpenKeyFile()
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
                AssemblyOriginatorKeyFile = paths[0];
            }
        }
    }
}
