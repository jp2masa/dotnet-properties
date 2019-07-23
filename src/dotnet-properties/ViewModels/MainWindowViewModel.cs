using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Microsoft.Build.Evaluation;

using ReactiveUI;

using DotNet.Properties.Dialogs.Models;
using DotNet.Properties.Dialogs.ViewModels;
using DotNet.Properties.Pages.ViewModels;
using DotNet.Properties.Services;

namespace DotNet.Properties.ViewModels
{
    internal class MainWindowViewModel : ReactiveObject
    {
        private const string AnyConfiguration = "Any Configuration";
        private const string AnyPlatform = "Any Platform";

        private readonly IDialogService<UnsavedChangesDialogViewModel> _unsavedChangesDialogService;

        private readonly Project _project;
        private readonly IPropertyManager _propertyManager;

        private readonly IThemeService _themeService;

        public MainWindowViewModel(
            MSBuildProject project,
            IDialogService<UnsavedChangesDialogViewModel> unsavedChangesDialogService,
            IOpenFileDialogService openFileDialogService,
            IThemeService themeService)
        {
            _project = project.Project;
            _propertyManager = new PropertyManager(_project);

            _unsavedChangesDialogService = unsavedChangesDialogService;

            ClosingCommand = ReactiveCommand.Create<CancelEventArgs>(OnClosing);

            SaveCommand = ReactiveCommand.Create(
                _propertyManager.Save,
                Observable.FromEventPattern(
                    handler => _propertyManager.IsDirtyChanged += handler,
                    handler => _propertyManager.IsDirtyChanged -= handler)
                    .Select(_ => _propertyManager.IsDirty));

            ApplicationPageViewModel = new ApplicationPageViewModel(_propertyManager);
            BuildPageViewModel = new BuildPageViewModel(_propertyManager);
            BuildEventsPageViewModel = new BuildEventsPageViewModel(_propertyManager);
            PackagePageViewModel = new PackagePageViewModel(_propertyManager);
            SigningPageViewModel = new SigningPageViewModel(_propertyManager, openFileDialogService);

            _themeService = themeService;
        }

        public ICommand ClosingCommand { get; }

        public string? ProjectPath => _project?.FullPath;
        public ICommand SaveCommand { get; }

        public IEnumerable<string> AvailableConfigurations =>
            _propertyManager.AvailableConfigurations.Append(AnyConfiguration);

        public IEnumerable<string> AvailablePlatforms =>
            _propertyManager.AvailablePlatforms.Append(AnyPlatform);

        public string Configuration
        {
            get => GetConfigurationDisplayName(_propertyManager.Configuration);
            set
            {
                _propertyManager.Configuration = value == AnyConfiguration ? String.Empty : value;
                this.RaisePropertyChanged();
            }
        }

        public string Platform
        {
            get => GetPlatformDisplayName(_propertyManager.Platform);
            set
            {
                _propertyManager.Platform = value == AnyPlatform ? String.Empty : value;
                this.RaisePropertyChanged();
            }
        }

        public ApplicationPageViewModel ApplicationPageViewModel { get; }
        public BuildPageViewModel BuildPageViewModel { get; }
        public BuildEventsPageViewModel BuildEventsPageViewModel { get; }
        public PackagePageViewModel PackagePageViewModel { get; }
        public SigningPageViewModel SigningPageViewModel { get; }

        public IEnumerable<ITheme> AvailableThemes => _themeService.AvailableThemes;

        public ITheme CurrentTheme
        {
            get => _themeService.CurrentTheme;
            set
            {
                _themeService.CurrentTheme = value;
                this.RaisePropertyChanged();
            }
        }

        private string GetConfigurationDisplayName(string? configuration) =>
            String.IsNullOrEmpty(configuration) ? AnyConfiguration : configuration;

        private string GetPlatformDisplayName(string? platform) =>
            String.IsNullOrEmpty(platform) ? AnyPlatform : platform;

        private void OnClosing(CancelEventArgs e)
        {
            if (_propertyManager.IsDirty)
            {
                var unsavedChangesDialogViewModel = new UnsavedChangesDialogViewModel();
                _unsavedChangesDialogService.Show(unsavedChangesDialogViewModel);

                switch (unsavedChangesDialogViewModel.DialogResult)
                {
                    case UnsavedChangesDialogResult.Yes:
                        _propertyManager.Save();
                        e.Cancel = false;
                        break;
                    case UnsavedChangesDialogResult.No:
                        e.Cancel = false;
                        break;
                    case UnsavedChangesDialogResult.Cancel:
                        e.Cancel = true;
                        break;
                    default:
                        throw new InvalidOperationException("Internal error!");
                }
            }
        }
    }
}
