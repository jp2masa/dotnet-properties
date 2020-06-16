using System;
using System.Collections.Generic;
using Microsoft.Build.Evaluation;

namespace DotNet.Properties.Services
{
    internal interface IPropertyManager
    {
        event EventHandler IsDirtyChanged;

        bool IsDirty { get; }

        IEnumerable<string> AvailableConfigurations { get; }
        IEnumerable<string> AvailablePlatforms { get; }

        string? Configuration { get; set; }
        string? Platform { get; set; }

        string? GetProperty(string propertyName, bool evaluatedValue = true);
        IReadOnlyCollection<ProjectItem> GetItems(string itemType);

        void SetProperty(string propertyName, string value);

        void Save();

        string MakeRelativePath(string? path);
    }
}
