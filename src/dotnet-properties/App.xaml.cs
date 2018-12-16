using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Loader;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging.Serilog;
using Avalonia.Markup.Xaml;

using Serilog;

using DotNet.Properties.Services;
using DotNet.Properties.Dialogs.Views;

namespace DotNet.Properties
{
    internal class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private static void Main()
        {
            InitializeLogging();

            var appBuilder = BuildAvaloniaApp();
            var app = (App)appBuilder.Instance;

            appBuilder.SetupWithoutStarting();

            var mainWindow = new MainWindow();

            if (TryBuildMainWindowDataContext(mainWindow, out var mainWindowDataContext))
            {
                mainWindow.DataContext = mainWindowDataContext;
                app.Run(mainWindow);
            }
        }

        private static bool TryBuildMainWindowDataContext(
            MainWindow mainWindow,
            out MainWindowViewModel viewModel)
        {
            var projFiles = Directory.GetFiles(Environment.CurrentDirectory, "*.*proj");
            var projectPath = projFiles.Length == 1 ? projFiles[0] : OpenProjectFile();

            if (!File.Exists(projectPath))
            {
                viewModel = null;
                return false;
            }

            IDotNetSdkResolver dotnetSdkResolver = new DotNetSdkResolver();
            dotnetSdkResolver.TryResolveSdkPath(Path.GetDirectoryName(projectPath), out var path);

            IMSBuildLoader msBuildLoader = new MSBuildLoader(path);

            AssemblyLoadContext.Default.Resolving += (context, name) =>
            {
                if(msBuildLoader.TryResolveMSBuildAssembly(context, name.Name, out var assembly))
                {
                    return assembly;
                }

                return null;
            };

            viewModel = new MainWindowViewModel(
                projectPath,
                dotnetSdkResolver,
                new DialogService<UnsavedChangesDialog>(() => new UnsavedChangesDialog(), mainWindow),
                new OpenFileDialogService(mainWindow));

            return true;
        }

        private static string OpenProjectFile() =>
            Task.Run(async () =>
            {
                var openFileDialog = new OpenFileDialog
                {
                    AllowMultiple = false,
                    Filters = new List<FileDialogFilter>()
                {
                    new FileDialogFilter() { Name = "All Project Files", Extensions = new List<string>() { "*proj" } },
                    new FileDialogFilter() { Name = "C# Project Files", Extensions = new List<string>() { "csproj" } },
                    new FileDialogFilter() { Name = "Visual Basic Project Files", Extensions = new List<string>() { "vbproj" } },
                    new FileDialogFilter() { Name = "F# Project Files", Extensions = new List<string>() { "fsproj" } },
                }
                };

                var result = await openFileDialog.ShowAsync().ConfigureAwait(false);

                if (result != null && result.Length > 0)
                {
                    return result[0];
                }

                return null;
            }).Result;

        private static AppBuilder BuildAvaloniaApp() =>
            AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI();

        [Conditional("DEBUG")]
        private static void InitializeLogging() =>
            SerilogLogger.Initialize(new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.Trace(outputTemplate: "{Area}: {Message}")
                .CreateLogger());
    }
}
