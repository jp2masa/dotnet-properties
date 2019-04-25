using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace DotNet.Properties.Services
{
    // Based on code from OmniSharp
    // https://github.com/OmniSharp/omnisharp-roslyn/blob/78ccc8b4376c73da282a600ac6fb10fce8620b52/src/OmniSharp.Abstractions/Services/DotNetCliService.cs
    internal sealed class DotNetSdkResolver : IDotNetSdkResolver
    {
        private readonly ConcurrentDictionary<string, DotNetInfo> _dotnetInfos = new ConcurrentDictionary<string, DotNetInfo>();

        public bool TryResolveSdkPath(string workingDirectory, out string? path)
        {
            if (_dotnetInfos.TryGetValue(workingDirectory, out var info))
            {
                path = info.BasePath;
                return true;
            }

            info = GetInfo(workingDirectory);

            if (info == null
                || !_dotnetInfos.TryAdd(workingDirectory, info))
            {
                path = null;
                return false;
            }

            path = info.BasePath;
            return true;
        }

        private static DotNetInfo GetInfo(string workingDirectory)
        {
            const string DOTNET_CLI_UI_LANGUAGE = nameof(DOTNET_CLI_UI_LANGUAGE);

            // Ensure that we set the DOTNET_CLI_UI_LANGUAGE environment variable to "en-US" before
            // running 'dotnet --info'. Otherwise, we may get localized results.
            var originalValue = Environment.GetEnvironmentVariable(DOTNET_CLI_UI_LANGUAGE);
            Environment.SetEnvironmentVariable(DOTNET_CLI_UI_LANGUAGE, "en-US");

            try
            {
                Process process;

                try
                {
                    var startInfo = new ProcessStartInfo("dotnet", "--info")
                    {
                        WorkingDirectory = workingDirectory,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };

                    process = Process.Start(startInfo);
                }
                catch (FileNotFoundException)
                {
                    return DotNetInfo.Empty;
                }
                catch (Win32Exception)
                {
                    return DotNetInfo.Empty;
                }

                if (process.HasExited)
                {
                    return DotNetInfo.Empty;
                }

                var lines = new List<string>();
                process.OutputDataReceived += (_, e) =>
                {
                    if (!String.IsNullOrWhiteSpace(e.Data))
                    {
                        lines.Add(e.Data);
                    }
                };

                process.BeginOutputReadLine();

                process.WaitForExit();

                return DotNetInfo.Parse(lines);
            }
            finally
            {
                Environment.SetEnvironmentVariable(DOTNET_CLI_UI_LANGUAGE, originalValue);
            }
        }
    }
}
