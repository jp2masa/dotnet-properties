using System;
using System.Runtime.CompilerServices;

using ReactiveUI;

using DotNet.Properties.DataAnnotations;
using DotNet.Properties.Services;

namespace DotNet.Properties.Pages.ViewModels
{
    internal sealed class PackagePageViewModel : PropertyPageViewModel
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
            public const string PackageLicenseFile = nameof(PackageLicenseFile);
            public const string PackageLicenseExpression = nameof(PackageLicenseExpression);
            public const string PackageRequireLicenseAcceptance = nameof(PackageRequireLicenseAcceptance);
            public const string PackageProjectUrl = nameof(PackageProjectUrl);
            public const string PackageIconUrl = nameof(PackageIconUrl);
            public const string PackageIcon = nameof(PackageIcon);
            public const string RepositoryUrl = nameof(RepositoryUrl);
            public const string RepositoryType = nameof(RepositoryType);
            public const string PackageTags = nameof(PackageTags);
            public const string PackageReleaseNotes = nameof(PackageReleaseNotes);
        }

        private bool? _isLicenseURL;
        private bool? _isLicenseFile;
        private bool? _isLicenseExpression;

        private string? _licenseURL;
        private string? _licenseFile;
        private string? _licenseExpression;

        private bool? _isIconUrl;
        private bool? _isIconFile;

        private string? _iconUrl;
        private string? _iconFile;

        public PackagePageViewModel(IPropertyManager propertyManager)
            : base(propertyManager)
        {
        }

        public bool GeneratePackageOnBuild
        {
            get => GetBooleanProperty(Property.GeneratePackageOnBuild);
            set => SetBooleanProperty(Property.GeneratePackageOnBuild, value);
        }

        public string? PackageId
        {
            get => GetStringProperty(Property.PackageId);
            set => SetStringProperty(Property.PackageId, value);
        }

        public string? PackageTitle
        {
            get => GetStringProperty(Property.Title);
            set => SetStringProperty(Property.Title, value);
        }

        public string? PackageVersion
        {
            get => GetStringProperty(Property.PackageVersion);
            set => SetStringProperty(Property.PackageVersion, value);
        }

        public string? Authors
        {
            get => GetStringProperty(Property.Authors);
            set => SetStringProperty(Property.Authors, value);
        }

        public string? Company
        {
            get => GetStringProperty(Property.Company);
            set => SetStringProperty(Property.Company, value);
        }

        public string? PackageDescription
        {
            get => GetStringProperty(Property.PackageDescription);
            set => SetStringProperty(Property.PackageDescription, value);
        }

        public string? Copyright
        {
            get => GetStringProperty(Property.Copyright);
            set => SetStringProperty(Property.Copyright, value);
        }

        public string? LicenseURL
        {
            get => GetStringProperty(Property.PackageLicenseUrl);
            set => UpdateProperty(ref _licenseURL, value, Property.PackageLicenseUrl);
        }

        public string? LicenseFile
        {
            get => GetStringProperty(Property.PackageLicenseFile);
            set => UpdateProperty(ref _licenseFile, value, Property.PackageLicenseFile);
        }

        [NuGetPackageLicenseExpression]
        public string? LicenseExpression
        {
            get => _licenseExpression ?? (_licenseExpression = GetStringProperty(Property.PackageLicenseExpression));
            set => UpdateProperty(ref _licenseExpression, value, Property.PackageLicenseExpression);
        }

        public bool IsLicenseURL
        {
            get => _isLicenseURL ?? (_isLicenseURL = !String.IsNullOrEmpty(LicenseURL)).Value;
            set => ChangeLicenseKind(_licenseURL, null, null, ref _isLicenseURL, value);
        }

        public bool IsLicenseFile
        {
            get => _isLicenseFile ?? (_isLicenseFile = !String.IsNullOrEmpty(LicenseFile)).Value;
            set => ChangeLicenseKind(null, _licenseFile, null, ref _isLicenseFile, value);
        }

        public bool IsLicenseExpression
        {
            get => _isLicenseExpression ?? (_isLicenseExpression = !String.IsNullOrEmpty(LicenseExpression)).Value;
            set => ChangeLicenseKind(null, null, _licenseExpression, ref _isLicenseExpression, value);
        }

        public bool RequireLicenseAcceptance
        {
            get => GetBooleanProperty(Property.PackageRequireLicenseAcceptance);
            set => SetBooleanProperty(Property.PackageRequireLicenseAcceptance, value);
        }

        public string? ProjectURL
        {
            get => GetStringProperty(Property.PackageProjectUrl);
            set => SetStringProperty(Property.PackageProjectUrl, value);
        }

        public string? IconUrl
        {
            get => GetStringProperty(Property.PackageIconUrl);
            set => UpdateProperty(ref _iconUrl, value, Property.PackageIconUrl);
        }

        public string? IconFile
        {
            get => GetStringProperty(Property.PackageIcon);
            set => UpdateProperty(ref _iconFile, value, Property.PackageIcon);
        }

        public bool IsIconUrl
        {
            get => _isIconUrl ?? (_isIconUrl = !String.IsNullOrEmpty(IconUrl)).Value;
            set => ChangeIconKind(_iconUrl, null, ref _isIconUrl, value);
        }

        public bool IsIconFile
        {
            get => _isIconFile ?? (_isIconFile = !String.IsNullOrEmpty(IconFile)).Value;
            set => ChangeIconKind(null, _iconFile, ref _isIconFile, value);
        }

        public string? RepositoryURL
        {
            get => GetStringProperty(Property.RepositoryUrl);
            set => SetStringProperty(Property.RepositoryUrl, value);
        }

        public string? RepositoryType
        {
            get => GetStringProperty(Property.RepositoryType);
            set => SetStringProperty(Property.RepositoryType, value);
        }

        public string? Tags
        {
            get => GetStringProperty(Property.PackageTags);
            set => SetStringProperty(Property.PackageTags, value);
        }

        public string? ReleaseNotes
        {
            get => GetStringProperty(Property.PackageReleaseNotes);
            set => SetStringProperty(Property.PackageReleaseNotes, value);
        }

        private void ChangeLicenseKind(
            string? licenseURL,
            string? licenseFile,
            string? licenseExpression,
            ref bool? isLicenseField,
            bool isLicense,
            [CallerMemberName] string? propertyName = null)
        {
            if (isLicense)
            {
                LicenseURL = licenseURL;
                LicenseFile = licenseFile;
                LicenseExpression = licenseExpression;
            }

            this.RaiseAndSetIfChanged(ref isLicenseField, isLicense, propertyName);
        }

        private void ChangeIconKind(
            string? iconUrl,
            string? icon,
            ref bool? isIconField,
            bool isIcon,
            [CallerMemberName] string? propertyName = null)
        {
            if (isIcon)
            {
                IconUrl = iconUrl;
                IconFile = icon;
            }

            this.RaiseAndSetIfChanged(ref isIconField, isIcon, propertyName);
        }

        private void UpdateProperty(
            ref string? field,
            string? value,
            string propertyName)
        {
            if (value != null)
            {
                field = value;
            }

            SetStringProperty(propertyName, value);
        }
    }
}
