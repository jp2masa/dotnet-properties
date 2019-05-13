using System.Collections.Generic;
using System.Linq;

using DotNet.Properties.Pages.Models;
using DotNet.Properties.Services;

namespace DotNet.Properties.Pages.ViewModels
{
    internal class BuildEventsPageViewModel : PropertyPageViewModel
    {
        private static class Property
        {
            public const string PreBuildEvent = nameof(PreBuildEvent);
            public const string PostBuildEvent = nameof(PostBuildEvent);
            public const string RunPostBuildEvent = nameof(RunPostBuildEvent);
        }

        private static readonly IEnumerable<RunPostBuildEvent> _runPostBuildEvent = new List<RunPostBuildEvent>()
        {
            new RunPostBuildEvent("OnBuildSuccess", "On Build Success"),
            new RunPostBuildEvent("OnOutputUpdated", "On Output Updated"),
            new RunPostBuildEvent("Always", "Always")
        };

        public BuildEventsPageViewModel(IPropertyManager propertyManager)
            : base(propertyManager)
        {
        }

        public string? PreBuildEvent
        {
            get => GetStringProperty(Property.PreBuildEvent);
            set => SetStringProperty(Property.PreBuildEvent, value);
        }

        public string? PostBuildEvent
        {
            get => GetStringProperty(Property.PostBuildEvent);
            set => SetStringProperty(Property.PostBuildEvent, value);
        }

        public IEnumerable<RunPostBuildEvent> SupportedRunPostBuildEvent => _runPostBuildEvent.Distinct();

        public RunPostBuildEvent RunPostBuildEvent
        {
            get => _runPostBuildEvent.Where(
                r => r.Value == GetStringProperty(Property.RunPostBuildEvent)).SingleOrDefault() ?? _runPostBuildEvent.ElementAt(0);
            set => SetStringProperty(Property.RunPostBuildEvent, value.Value);
        }
    }
}
