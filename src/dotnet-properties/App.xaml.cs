using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Platform;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.Threading;

using DotNet.Properties.Dialogs.ViewModels;
using DotNet.Properties.Dialogs.Views;
using DotNet.Properties.Services;
using DotNet.Properties.ViewModels;
using DotNet.Properties.Views;

namespace DotNet.Properties
{
    internal class App : Application
    {
        private static readonly List<FileDialogFilter> OpenProjectFileDialogFilters = new List<FileDialogFilter>()
        {
            new FileDialogFilter() { Name = "All Project Files", Extensions = new List<string>() { "*proj" } },
            new FileDialogFilter() { Name = "C# Project Files", Extensions = new List<string>() { "csproj" } },
            new FileDialogFilter() { Name = "Visual Basic Project Files", Extensions = new List<string>() { "vbproj" } },
            new FileDialogFilter() { Name = "F# Project Files", Extensions = new List<string>() { "fsproj" } },
        };

        public override void Initialize() => AvaloniaXamlLoader.Load(this);

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = new MainWindow();
                var mainWindowViewModel = BuildMainWindowDataContext(mainWindow);

                if (mainWindowViewModel != null)
                {
                    mainWindow.DataContext = mainWindowViewModel;
                    desktop.MainWindow = mainWindow;
                }
            }

            base.OnFrameworkInitializationCompleted();
        }

        private static int Main(string[] args) =>
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args, ShutdownMode.OnMainWindowClose);

        private MainWindowViewModel? BuildMainWindowDataContext(MainWindow mainWindow)
        {
            var projFiles = Directory.GetFiles(Environment.CurrentDirectory, "*.*proj");

            var projectPath = projFiles.Length == 1 ? projFiles[0] : OpenProjectFile();
            var projectDirectory = Path.GetDirectoryName(projectPath);

            // TODO: replace with File.Exists and Directory.Exists when nullable annotations are correct
            if (!FileExists(projectPath) || !DirectoryExists(projectDirectory))
            {
                return null;
            }

            IDotNetSdkResolver dotnetSdkResolver = new DotNetSdkResolver();

            if (!dotnetSdkResolver.TryResolveSdkPath(projectDirectory, out var dotnetSdkPath))
            {
                return null;
            }

            var dotnetSdkPaths = new DotNetSdkPaths(dotnetSdkPath);

            IMSBuildLoader msBuildLoader = new MSBuildLoader(dotnetSdkPaths);

            AssemblyLoadContext.Default.Resolving += (context, name) =>
            {
                if (name.Name != null && msBuildLoader.TryResolveMSBuildAssembly(context, name.Name, out var assembly))
                {
                    return assembly;
                }

                return null;
            };

            var msBuildProject = new MSBuildProject(dotnetSdkPaths, projectPath);

            return new MainWindowViewModel(
                msBuildProject,
                new DialogService<UnsavedChangesDialog, UnsavedChangesDialogViewModel>(NewUnsavedChangesDialog, mainWindow),
                new OpenFileDialogService(mainWindow),
                new ThemeService(this));
        }

        private static string? OpenProjectFile()
        {
            Task<string?> task;

            using (var source = new CancellationTokenSource())
            {
                task = OpenProjectFileAsync();
                task.ContinueWith(t => source.Cancel(), TaskScheduler.FromCurrentSynchronizationContext());

                Dispatcher.UIThread.MainLoop(source.Token);
            }

            return task.Result;
        }

        private static async Task<string?> OpenProjectFileAsync()
        {
            var openFileDialog = new OpenFileDialog
            {
                AllowMultiple = false,
                Filters = OpenProjectFileDialogFilters
            };

            var result = await ShowOpenFileDialogAsync(openFileDialog).ConfigureAwait(false);

            if (result != null && result.Length > 0)
            {
                return result[0];
            }

            return null;
        }

        private static AppBuilder BuildAvaloniaApp() =>
            AppBuilder.Configure<App>()
                .UsePlatformDetect()
#if DEBUG
                .LogToDebug()
#endif
                .UseReactiveUI();

        private static UnsavedChangesDialog NewUnsavedChangesDialog() => new UnsavedChangesDialog();

        private static bool DirectoryExists([NotNullWhen(true)] string? path) => Directory.Exists(path);

        private static bool FileExists([NotNullWhen(true)] string? path) => File.Exists(path);

        private static Task<string[]> ShowOpenFileDialogAsync(FileDialog dialog, Window? parent = null)
        {
            var systemDialogImpl = AvaloniaLocator.Current.GetService<ISystemDialogImpl>();
            return systemDialogImpl.ShowFileDialogAsync(dialog, parent);
        }
    }
}
