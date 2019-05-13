using System;
using System.Collections.Generic;
using System.Linq;

using DotNet.Properties.Pages.Models;
using DotNet.Properties.Services;

namespace DotNet.Properties.Pages.ViewModels
{
    internal class ApplicationPageViewModel : PropertyPageViewModel
    {
        private static class Property
        {
            public const string AssemblyName = nameof(AssemblyName);
            public const string RootNamespace = nameof(RootNamespace);
            public const string TargetFramework = nameof(TargetFramework);
            public const string OutputType = nameof(OutputType);
        }

        private readonly Lazy<IEnumerable<TargetFramework>> _supportedTargetFrameworks;

        private static readonly IEnumerable<OutputType> _outputTypes = new List<OutputType>()
        {
            new OutputType("Exe", "Console Application"),
            new OutputType("WinExe", "Windows Application"),
            new OutputType("Library", "Class Library")
        };

        public ApplicationPageViewModel(IPropertyManager propertyManager)
            : base(propertyManager)
        {
            _supportedTargetFrameworks = new Lazy<IEnumerable<TargetFramework>>(
                () => GetItems("SupportedTargetFramework").Select(
                    i => new TargetFramework(i.EvaluatedInclude, i.GetMetadata("DisplayName").EvaluatedValue)));
        }

        public string? AssemblyName
        {
            get => GetStringProperty(Property.AssemblyName);
            set => SetStringProperty(Property.AssemblyName, value);
        }

        public string? DefaultNamespace
        {
            get => GetStringProperty(Property.RootNamespace);
            set => SetStringProperty(Property.RootNamespace, value);
        }

        public IEnumerable<TargetFramework> SupportedTargetFrameworks => _supportedTargetFrameworks.Value;

        public TargetFramework TargetFramework
        {
            get => _supportedTargetFrameworks.Value.Where(f => f.ShortName == GetStringProperty(Property.TargetFramework)).Single();
            set => SetStringProperty(Property.TargetFramework, value.ShortName);
        }

        public IEnumerable<OutputType> SupportedOutputTypes => _outputTypes;

        public OutputType OutputType
        {
            get => _outputTypes.Where(t => t.Value == GetStringProperty(Property.OutputType)).Single();
            set => SetStringProperty(Property.OutputType, value.Value);
        }
    }
}
