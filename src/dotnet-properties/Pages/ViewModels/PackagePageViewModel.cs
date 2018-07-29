using DotNet.Properties.Services;

namespace DotNet.Properties.Pages.ViewModels
{
    internal class PackagePageViewModel : PropertyPageViewModel
    {
        private static class Property
        {
            public const string GeneratePackageOnBuild = nameof(GeneratePackageOnBuild);
            public const string PackageId = nameof(PackageId);
            public const string Title = nameof(Title);
            public const string PackageVersion = nameof(PackageVersion);
            public const string Authors = nameof(Authors);
            public const string Company = nameof(Company);
            public const string PackageDescription = nameof(PackageDescription);
            public const string Copyright = nameof(Copyright);
            public const string PackageLicenseUrl = nameof(PackageLicenseUrl);
            public const string PackageRequireLicenseAcceptance = nameof(PackageRequireLicenseAcceptance);
            public const string PackageProjectUrl = nameof(PackageProjectUrl);
            public const string PackageIconUrl = nameof(PackageIconUrl);
            public const string RepositoryUrl = nameof(RepositoryUrl);
            public const string RepositoryType = nameof(RepositoryType);
            public const string PackageTags = nameof(PackageTags);
            public const string PackageReleaseNotes = nameof(PackageReleaseNotes);
        }

        public PackagePageViewModel(IPropertyManager propertyManager)
            : base(propertyManager)
        {
        }

        public bool GeneratePackageOnBuild
        {
            get => GetBooleanProperty(Property.GeneratePackageOnBuild);
            set => SetBooleanProperty(Property.GeneratePackageOnBuild, value);
        }

        public string PackageId
        {
            get => GetStringProperty(Property.PackageId);
            set => SetStringProperty(Property.PackageId, value);
        }

        public string PackageTitle
        {
            get => GetStringProperty(Property.Title);
            set => SetStringProperty(Property.Title, value);
        }

        public string PackageVersion
        {
            get => GetStringProperty(Property.PackageVersion);
            set => SetStringProperty(Property.PackageVersion, value);
        }

        public string Authors
        {
            get => GetStringProperty(Property.Authors);
            set => SetStringProperty(Property.Authors, value);
        }

        public string Company
        {
            get => GetStringProperty(Property.Company);
            set => SetStringProperty(Property.Company, value);
        }

        public string PackageDescription
        {
            get => GetStringProperty(Property.PackageDescription);
            set => SetStringProperty(Property.PackageDescription, value);
        }

        public string Copyright
        {
            get => GetStringProperty(Property.Copyright);
            set => SetStringProperty(Property.Copyright, value);
        }

        public string LicenseURL
        {
            get => GetStringProperty(Property.PackageLicenseUrl);
            set => SetStringProperty(Property.PackageLicenseUrl, value);
        }

        public bool RequireLicenseAcceptance
        {
            get => GetBooleanProperty(Property.PackageRequireLicenseAcceptance);
            set => SetBooleanProperty(Property.PackageRequireLicenseAcceptance, value);
        }

        public string ProjectURL
        {
            get => GetStringProperty(Property.PackageProjectUrl);
            set => SetStringProperty(Property.PackageProjectUrl, value);
        }

        public string IconURL
        {
            get => GetStringProperty(Property.PackageIconUrl);
            set => SetStringProperty(Property.PackageIconUrl, value);
        }

        public string RepositoryURL
        {
            get => GetStringProperty(Property.RepositoryUrl);
            set => SetStringProperty(Property.RepositoryUrl, value);
        }

        public string RepositoryType
        {
            get => GetStringProperty(Property.RepositoryType);
            set => SetStringProperty(Property.RepositoryType, value);
        }

        public string Tags
        {
            get => GetStringProperty(Property.PackageTags);
            set => SetStringProperty(Property.PackageTags, value);
        }

        public string ReleaseNotes
        {
            get => GetStringProperty(Property.PackageReleaseNotes);
            set => SetStringProperty(Property.PackageReleaseNotes, value);
        }
    }
}
