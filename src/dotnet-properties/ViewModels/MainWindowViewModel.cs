using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Build.Evaluation;

using DotNet.Properties.Dialogs.Models;
using DotNet.Properties.Dialogs.ViewModels;
using DotNet.Properties.Pages.ViewModels;
using DotNet.Properties.Services;

namespace DotNet.Properties
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private const string AnyConfiguration = "Any Configuration";
        private const string AnyPlatform = "Any Platform";

        private MSBuildProject _msBuildProject;
        private Project _project;

        private IPropertyManager _propertyManager;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel(
            string projectPath,
            IDialogService unsavedChangesDialogService,
            IOpenFileDialogService openFileDialogService)
        {
            if (!File.Exists(projectPath))
            {
                throw new FileNotFoundException("Project file not found!", projectPath);
            }

            _msBuildProject = new MSBuildProject(projectPath);

            _project = _msBuildProject.Project;
            _propertyManager = new PropertyManager(_project);

            var saveCommand = new SaveCommand(_propertyManager);

            ClosingCommand = new ClosingCommand(unsavedChangesDialogService, saveCommand);
            SaveCommand = saveCommand;

            ApplicationPageViewModel = new ApplicationPageViewModel(_propertyManager);
            BuildPageViewModel = new BuildPageViewModel(_propertyManager);
            BuildEventsPageViewModel = new BuildEventsPageViewModel(_propertyManager);
            PackagePageViewModel = new PackagePageViewModel(_propertyManager);
            SigningPageViewModel = new SigningPageViewModel(_propertyManager, openFileDialogService);
        }

        public ICommand ClosingCommand { get; }

        public string ProjectPath => _project?.FullPath;
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
                OnPropertyChanged();
            }
        }

        public string Platform
        {
            get => GetPlatformDisplayName(_propertyManager.Platform);
            set
            {
                _propertyManager.Platform = value == AnyPlatform ? String.Empty : value;
                OnPropertyChanged();
            }
        }

        public ApplicationPageViewModel ApplicationPageViewModel { get; }
        public BuildPageViewModel BuildPageViewModel { get; }
        public BuildEventsPageViewModel BuildEventsPageViewModel { get; }
        public PackagePageViewModel PackagePageViewModel { get; }
        public SigningPageViewModel SigningPageViewModel { get; }

        private void SetProperty<T>(ref T propertyRef, T value, [CallerMemberName]string changedProperty = null)
        {
            if (!EqualityComparer<T>.Default.Equals(propertyRef, value))
            {
                propertyRef = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(changedProperty));
            }
        }

        private void OnPropertyChanged([CallerMemberName]string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private string GetConfigurationDisplayName(string configuration) =>
            String.IsNullOrEmpty(configuration) ? AnyConfiguration : configuration;

        private string GetPlatformDisplayName(string platform) =>
            String.IsNullOrEmpty(platform) ? AnyPlatform : platform;
    }

    internal class ClosingCommand : ICommand
    {
        private readonly IDialogService _unsavedChangesDialogFactory;
        private readonly SaveCommand _saveCommand;

#pragma warning disable CS0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067

        public ClosingCommand(
            IDialogService unsavedChangesDialogFactory,
            SaveCommand saveCommand)
        {
            _unsavedChangesDialogFactory = unsavedChangesDialogFactory;
            _saveCommand = saveCommand;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (!(parameter is CancelEventArgs cancelEventArgs))
            {
                throw new InvalidOperationException("Internal error!");
            }

            var unsavedChangesDialogViewModel = new UnsavedChangesDialogViewModel();
            _unsavedChangesDialogFactory.Show(unsavedChangesDialogViewModel);

            switch (unsavedChangesDialogViewModel.DialogResult)
            {
                case UnsavedChangesDialogResult.Yes:
                    _saveCommand.Execute(null);
                    cancelEventArgs.Cancel = false;
                    break;
                case UnsavedChangesDialogResult.No:
                    cancelEventArgs.Cancel = false;
                    break;
                case UnsavedChangesDialogResult.Cancel:
                    cancelEventArgs.Cancel = true;
                    break;
                default:
                    throw new InvalidOperationException("Internal error!");
            }
        }
    }

    internal class SaveCommand : ICommand
    {
        private IPropertyManager _propertyManager;

        public event EventHandler CanExecuteChanged;

        public SaveCommand(IPropertyManager propertyManager)
        {
            _propertyManager = propertyManager ?? throw new ArgumentNullException(nameof(propertyManager));

            _propertyManager.IsDirtyChanged += delegate
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            };
        }

        public bool CanExecute(object parameter) => _propertyManager.IsDirty;

        public void Execute(object parameter) => _propertyManager.Save();
    }
}
