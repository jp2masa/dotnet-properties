using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        private readonly Lazy<ImmutableArray<TargetFramework>> _supportedTargetFrameworks;

        private static readonly ImmutableArray<OutputType> _outputTypes =
            ImmutableArray.Create(
                new OutputType("Exe", "Console Application"),
                new OutputType("WinExe", "Windows Application"),
                new OutputType("Library", "Class Library"));

        private static readonly ImmutableArray<DotNetFrameworkTfm> _dotNetFrameworkTfms =
            ImmutableArray.Create(
                new DotNetFrameworkTfm("net20", new Version(2, 0)),
                new DotNetFrameworkTfm("net40", new Version(4, 0)),
                new DotNetFrameworkTfm("net45", new Version(4, 5)),
                new DotNetFrameworkTfm("net451", new Version(4, 5, 1)),
                new DotNetFrameworkTfm("net452", new Version(4, 5, 2)),
                new DotNetFrameworkTfm("net46", new Version(4, 6)),
                new DotNetFrameworkTfm("net461", new Version(4, 6, 1)),
                new DotNetFrameworkTfm("net462", new Version(4, 6, 2)),
                new DotNetFrameworkTfm("net47", new Version(4, 7)),
                new DotNetFrameworkTfm("net471", new Version(4, 7, 1)),
                new DotNetFrameworkTfm("net472", new Version(4, 7, 2)),
                new DotNetFrameworkTfm("net48", new Version(4, 8)));

        public ApplicationPageViewModel(IPropertyManager propertyManager)
            : base(propertyManager)
        {
            _supportedTargetFrameworks = new Lazy<ImmutableArray<TargetFramework>>(GetAvailableFrameworks);
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

        private ImmutableArray<TargetFramework> GetAvailableFrameworks()
        {
            var supportedTargetFrameworkItems = GetItems("SupportedTargetFramework");

            var builder = ImmutableArray.CreateBuilder<TargetFramework>(
                supportedTargetFrameworkItems.Count + _dotNetFrameworkTfms.Length);

            builder.AddRange(
                from item in supportedTargetFrameworkItems
                select new TargetFramework(item.EvaluatedInclude, item.GetMetadata("DisplayName").EvaluatedValue));

            builder.AddRange(
                from tfm in _dotNetFrameworkTfms
                select new TargetFramework(tfm.ShortName, $".NET Framework {tfm.Version}"));

            return builder.ToImmutable();
        }
    }
}
