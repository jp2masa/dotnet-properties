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
        private IPropertyManager _propertyManager;

        public PropertyPageViewModel(IPropertyManager propertyManager)
        {
            _propertyManager = propertyManager;
        }
        
        protected bool GetBooleanProperty(string propertyName)
        {
            var value = _propertyManager.GetProperty(propertyName);
            return String.Equals(value, Boolean.TrueString, StringComparison.InvariantCultureIgnoreCase);
        }

        protected string GetStringProperty(string propertyName) => _propertyManager.GetProperty(propertyName);

        protected IEnumerable<ProjectItem> GetItems(string itemType) =>
            _propertyManager.GetItems(itemType);

        protected void SetBooleanProperty(string propertyName, bool value, [CallerMemberName] string changedProperty = null)
        {
            _propertyManager.SetProperty(propertyName, value.ToString(CultureInfo.InvariantCulture));
            this.RaisePropertyChanged(changedProperty);
        }

        protected void SetStringProperty(string propertyName, string value, [CallerMemberName] string changedProperty = null)
        {
            _propertyManager.SetProperty(propertyName, value ?? String.Empty);
            this.RaisePropertyChanged(changedProperty);
        }

        protected void Save() => _propertyManager.Save();
    }
}
