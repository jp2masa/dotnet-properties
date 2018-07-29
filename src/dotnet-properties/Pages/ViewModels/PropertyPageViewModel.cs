using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.Build.Evaluation;

using DotNet.Properties.Services;

namespace DotNet.Properties.Pages.ViewModels
{
    internal abstract class PropertyPageViewModel : INotifyPropertyChanged
    {
        private IPropertyManager _propertyManager;

        public event PropertyChangedEventHandler PropertyChanged;

        public PropertyPageViewModel(IPropertyManager propertyManager)
        {
            _propertyManager = propertyManager;
        }

        protected void OnPropertyChanged([CallerMemberName]string property = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(changedProperty));
        }

        protected void SetStringProperty(string propertyName, string value, [CallerMemberName] string changedProperty = null)
        {
            _propertyManager.SetProperty(propertyName, value ?? String.Empty);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(changedProperty));
        }

        protected void Save() => _propertyManager.Save();
    }
}
