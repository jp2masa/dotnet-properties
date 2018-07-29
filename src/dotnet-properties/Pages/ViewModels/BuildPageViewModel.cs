using System.Collections.Generic;
using System.Linq;

using DotNet.Properties.Pages.Models;
using DotNet.Properties.Services;

namespace DotNet.Properties.Pages.ViewModels
{
    internal class BuildPageViewModel : PropertyPageViewModel
    {
        private static class Property
        {
            public const string DefineConstants = nameof(DefineConstants);
            public const string PlatformTarget = nameof(PlatformTarget);
            public const string Prefer32Bit = nameof(Prefer32Bit);
            public const string AllowUnsafeBlocks = nameof(AllowUnsafeBlocks);
            public const string Optimize = nameof(Optimize);
            public const string OutputPath = nameof(OutputPath);
            public const string AppendTargetFrameworkToOutputPath = nameof(AppendTargetFrameworkToOutputPath);
            public const string AppendRuntimeIdentifierToOutputPath = nameof(AppendRuntimeIdentifierToOutputPath);
            public const string GenerateDocumentationFile = nameof(GenerateDocumentationFile);
            public const string DocumentationFile = nameof(DocumentationFile);
        }

        private static IEnumerable<PlatformTarget> _platformTargets = new List<PlatformTarget>()
        {
            new PlatformTarget("AnyCPU", "Any CPU"),
            new PlatformTarget("x64", "x64"),
            new PlatformTarget("x86", "x86"),
            new PlatformTarget("ARM", "ARM")
        };

        public BuildPageViewModel(IPropertyManager propertyManager)
            : base(propertyManager)
        {
        }

        public string ConditionalCompilationSymbols
        {
            get => GetStringProperty(Property.DefineConstants);
            set => SetStringProperty(Property.DefineConstants, value);
        }

        public IEnumerable<PlatformTarget> AvailablePlatformTargets => _platformTargets;

        public bool PlatformTargetEnabled { get; } = true;

        public PlatformTarget PlatformTarget
        {
            get =>_platformTargets.Single(t => t.Value == (GetStringProperty(Property.PlatformTarget) ?? "AnyCPU"));
            set => SetStringProperty(Property.PlatformTarget, value.Value);
        }

        public bool Prefer32Bit
        {
            get => GetBooleanProperty(Property.Prefer32Bit);
            set => SetBooleanProperty(Property.Prefer32Bit, value);
        }

        public bool AllowUnsafeCode
        {
            get => GetBooleanProperty(Property.AllowUnsafeBlocks);
            set => SetBooleanProperty(Property.AllowUnsafeBlocks, value);
        }

        public bool OptimizeCode
        {
            get => GetBooleanProperty(Property.Optimize);
            set => SetBooleanProperty(Property.Optimize, value);
        }

        public string OutputPath
        {
            get => GetStringProperty(Property.OutputPath);
            set => SetStringProperty(Property.OutputPath, value);
        }

        public bool AppendTargetFrameworkToOutputPath
        {
            get => GetBooleanProperty(Property.AppendTargetFrameworkToOutputPath);
            set
            {
                SetBooleanProperty(Property.AppendTargetFrameworkToOutputPath, value);
                OnPropertyChanged(nameof(OutputPath));
            }
        }

        public bool AppendRuntimeIdentifierToOutputPath
        {
            get => GetBooleanProperty(Property.AppendRuntimeIdentifierToOutputPath);
            set
            {
                SetBooleanProperty(Property.AppendRuntimeIdentifierToOutputPath, value);
                OnPropertyChanged(nameof(OutputPath));
            }
        }

        public bool GenerateDocumentationFile
        {
            get => GetBooleanProperty(Property.GenerateDocumentationFile);
            set => SetBooleanProperty(Property.GenerateDocumentationFile, value);
        }

        public string DocumentationFile
        {
            get => GetStringProperty(Property.DocumentationFile);
            set => SetStringProperty(Property.DocumentationFile, value);
        }
    }
}
