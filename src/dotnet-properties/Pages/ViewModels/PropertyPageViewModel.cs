using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.Build.Evaluation;

using ReactiveUI;

using DotNet.Properties.Services;

namespace DotNet.Properties.Pages.ViewModels
{
    internal abstract class PropertyPageViewModel : ReactiveObject
    {
        protected PropertyPageViewModel(IPropertyManager propertyManager)
        {
            PropertyManager = propertyManager;
        }

        protected IPropertyManager PropertyManager { get; }

        protected bool GetBooleanProperty(string propertyName)
        {
            var value = PropertyManager.GetProperty(propertyName);
            return String.Equals(value, Boolean.TrueString, StringComparison.InvariantCultureIgnoreCase);
        }

        protected string? GetStringProperty(string propertyName) => PropertyManager.GetProperty(propertyName);

        protected IReadOnlyCollection<ProjectItem> GetItems(string itemType) =>
            PropertyManager.GetItems(itemType);

        protected void SetBooleanProperty(string propertyName, bool value, [CallerMemberName] string? changedProperty = null)
        {
            PropertyManager.SetProperty(propertyName, value.ToString(CultureInfo.InvariantCulture));
            this.RaisePropertyChanged(changedProperty);
        }

        protected void SetStringProperty(string propertyName, string? value, [CallerMemberName] string? changedProperty = null)
        {
            PropertyManager.SetProperty(propertyName, value ?? String.Empty);
            this.RaisePropertyChanged(changedProperty);
        }

        protected void Save() => PropertyManager.Save();
    }
}
