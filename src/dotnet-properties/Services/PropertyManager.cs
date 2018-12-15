using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace DotNet.Properties.Services
{
    internal class PropertyManager : IPropertyManager
    {
        private const string ConfigurationCondition = "'$(Configuration)' == '{0}'";
        private const string PlatformCondition = "'$(Platform)' == '{0}'";
        private const string ConfigurationAndPlatformCondition = "'$(Configuration)|$(Platform)' == '{0}|{1}'";

        public event EventHandler IsDirtyChanged;

        public bool IsDirty => _project.Xml.HasUnsavedChanges;

        public IEnumerable<string> AvailableConfigurations =>
            _project.GetPropertyValue("Configurations").Split(';').Select(c => c.Trim());

        public IEnumerable<string> AvailablePlatforms =>
            _project.GetPropertyValue("Platforms").Split(';').Select(c => c.Trim());

        public string Configuration
        {
            get => _configuration;
            set => SetConfiguration(value);
        }

        public string Platform
        {
            get => _platform;
            set => SetPlatform(value);
        }

        private Project _project;

        private string _configuration;
        private string _platform;

        public PropertyManager(Project project)
        {
            _project = project ?? throw new ArgumentNullException(nameof(project));

            SetConfiguration(String.Empty, false);
            SetPlatform(String.Empty, false);

            _project.ReevaluateIfNecessary();
        }

        public string GetProperty(
            string propertyName,
            bool evaluatedValue = true)
        {
            var property = _project.GetProperty(propertyName);
            return evaluatedValue ? property?.EvaluatedValue : property?.UnevaluatedValue;
        }

        public IEnumerable<ProjectItem> GetItems(string itemType) => _project.GetItems(itemType);

        public void SetProperty(string propertyName, string value)
        {
            var isAnyConfiguration = IsAny(_configuration);
            var isAnyPlatform = IsAny(_platform);

            if (!isAnyConfiguration || !isAnyPlatform)
            {
                ProjectPropertyGroupElement propertyGroup;

                if (isAnyConfiguration)
                {
                    propertyGroup = GetPropertyGroupForPlatform(_platform);
                }
                else if (isAnyPlatform)
                {
                    propertyGroup = GetPropertyGroupForConfiguration(_configuration);
                }
                else
                {
                    propertyGroup = GetPropertyGroupForConfigurationAndPlatform(_configuration, _platform);
                }

                propertyGroup.SetProperty(propertyName, value);

                _project.ReevaluateIfNecessary();
            }
            else
            {
                _project.SetProperty(propertyName, value);
            }

            OnIsDirtyChanged();
        }

        public void Save()
        {
            _project.Save();
            _project.ReevaluateIfNecessary();

            OnIsDirtyChanged();
        }

        private void SetConfiguration(string configuration, bool reevaluateIfNecessary = true)
        {
            if (_configuration != configuration)
            {
                _project.SetGlobalProperty("Configuration", configuration);
                _configuration = configuration;

                if (reevaluateIfNecessary)
                {
                    _project.ReevaluateIfNecessary();
                }
            }
        }

        private void SetPlatform(string platform, bool reevaluateIfNecessary = true)
        {
            if (_platform != platform)
            {
                _project.SetGlobalProperty("Platform", platform);
                _platform = platform;

                if (reevaluateIfNecessary)
                {
                    _project.ReevaluateIfNecessary();
                }
            }
        }

        private ProjectPropertyGroupElement CreatePropertyGroup(string condition)
        {
            var propertyGroupElement = _project.Xml.AddPropertyGroup();
            propertyGroupElement.Condition = condition;

            return propertyGroupElement;
        }

        private ProjectPropertyGroupElement GetPropertyGroup(string condition) =>
            _project.Xml.PropertyGroups.Where(p => p.Condition.Trim() == condition).LastOrDefault()
         ?? CreatePropertyGroup(condition);

        private ProjectPropertyGroupElement GetPropertyGroupForConfiguration(string configuration) =>
            GetPropertyGroup(String.Format(CultureInfo.InvariantCulture, ConfigurationCondition, configuration));

        private ProjectPropertyGroupElement GetPropertyGroupForPlatform(string platform) =>
            GetPropertyGroup(Format(PlatformCondition, platform));

        private ProjectPropertyGroupElement GetPropertyGroupForConfigurationAndPlatform(
            string configuration, string platform) =>
            GetPropertyGroup(Format(ConfigurationAndPlatformCondition, configuration, platform));

        private void OnIsDirtyChanged() => IsDirtyChanged?.Invoke(this, EventArgs.Empty);

        private static bool IsAny(string value) => String.IsNullOrEmpty(value);

        private static string Format(string format, object arg0) =>
            String.Format(CultureInfo.InvariantCulture, format, arg0);

        private static string Format(string format, object arg0, object arg1) =>
            String.Format(CultureInfo.InvariantCulture, format, arg0, arg1);
    }
}
