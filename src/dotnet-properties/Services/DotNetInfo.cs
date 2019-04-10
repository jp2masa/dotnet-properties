using System;
using System.Collections.Generic;

using NuGet.Versioning;

namespace DotNet.Properties.Services
{
    // Code from OmniSharp: https://github.com/OmniSharp/omnisharp-roslyn/blob/1cc5321d320020f07c89969ef06eb52fe8c77379/src/OmniSharp.Abstractions/Services/DotNetInfo.cs
    internal sealed class DotNetInfo
    {
        public static DotNetInfo Empty { get; } = new DotNetInfo();

        public bool IsEmpty { get; }

        public SemanticVersion? Version { get; }
        public string? OSName { get; }
        public string? OSVersion { get; }
        public string? OSPlatform { get; }
        public string? RID { get; }
        public string? BasePath { get; }

        private DotNetInfo()
        {
            IsEmpty = true;
        }

        private DotNetInfo(string version, string osName, string osVersion, string osPlatform, string rid, string basePath)
        {
            IsEmpty = false;

            Version = SemanticVersion.TryParse(version, out var value) ? value : null;

            OSName = osName;
            OSVersion = osVersion;
            OSPlatform = osPlatform;
            RID = rid;
            BasePath = basePath;
        }

        public static DotNetInfo Parse(List<string> lines)
        {
            if (lines == null || lines.Count == 0)
            {
                return Empty;
            }

            var version = String.Empty;
            var osName = String.Empty;
            var osVersion = String.Empty;
            var osPlatform = String.Empty;
            var rid = String.Empty;
            var basePath = String.Empty;

            foreach (var line in lines)
            {
                var colonIndex = line.IndexOf(':', StringComparison.OrdinalIgnoreCase);

                if (colonIndex >= 0)
                {
                    var name = line.Substring(0, colonIndex).Trim();
                    var value = line.Substring(colonIndex + 1).Trim();

                    if (String.IsNullOrEmpty(version) && name.Equals("Version", StringComparison.OrdinalIgnoreCase))
                    {
                        version = value;
                    }
                    else if (String.IsNullOrEmpty(osName) && name.Equals("OS Name", StringComparison.OrdinalIgnoreCase))
                    {
                        osName = value;
                    }
                    else if (String.IsNullOrEmpty(osVersion) && name.Equals("OS Version", StringComparison.OrdinalIgnoreCase))
                    {
                        osVersion = value;
                    }
                    else if (String.IsNullOrEmpty(osPlatform) && name.Equals("OS Platform", StringComparison.OrdinalIgnoreCase))
                    {
                        osPlatform = value;
                    }
                    else if (String.IsNullOrEmpty(rid) && name.Equals("RID", StringComparison.OrdinalIgnoreCase))
                    {
                        rid = value;
                    }
                    else if (String.IsNullOrEmpty(basePath) && name.Equals("Base Path", StringComparison.OrdinalIgnoreCase))
                    {
                        basePath = value;
                    }
                }
            }

            if (String.IsNullOrWhiteSpace(version) &&
                String.IsNullOrWhiteSpace(osName) &&
                String.IsNullOrWhiteSpace(osVersion) &&
                String.IsNullOrWhiteSpace(osPlatform) &&
                String.IsNullOrWhiteSpace(rid) &&
                String.IsNullOrWhiteSpace(basePath))
            {
                return Empty;
            }

            return new DotNetInfo(version, osName, osVersion, osPlatform, rid, basePath);
        }
    }
}
