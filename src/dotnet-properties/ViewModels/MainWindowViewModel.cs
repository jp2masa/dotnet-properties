using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            _themeService = themeService;

            ClosingCommand = ReactiveCommand.Create<CancelEventArgs>(OnClosing);

            SaveCommand = ReactiveCommand.Create(
                _propertyManager.Save,
                Observable.FromEventPattern(
                    handler => _propertyManager.IsDirtyChanged += handler,
                    handler => _propertyManager.IsDirtyChanged -= handler)
                    .Select(_ => _propertyManager.IsDirty));

            ApplicationPage = new ApplicationPageViewModel(_propertyManager);
            BuildPage = new BuildPageViewModel(_propertyManager);
            BuildEventsPage = new BuildEventsPageViewModel(_propertyManager);
            PackagePage = new PackagePageViewModel(_propertyManager);
            SigningPage = new SigningPageViewModel(_propertyManager, openFileDialogService);
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

        public ApplicationPageViewModel ApplicationPage { get; }
        public BuildPageViewModel BuildPage { get; }
        public BuildEventsPageViewModel BuildEventsPage { get; }
        public PackagePageViewModel PackagePage { get; }
        public SigningPageViewModel SigningPage { get; }

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
