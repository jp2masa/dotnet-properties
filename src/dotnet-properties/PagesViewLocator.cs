using System;
using System.Collections.Generic;

using Avalonia.Controls;
using Avalonia.Controls.Templates;

using DotNet.Properties.Pages.ViewModels;
using DotNet.Properties.Pages.Views;

namespace DotNet.Properties
{
    internal sealed class PagesViewLocator : IDataTemplate
    {
        private static readonly Dictionary<Type, Func<IControl>> Templates = new Dictionary<Type, Func<IControl>>()
        {
            { typeof(ApplicationPageViewModel), New<ApplicationPage> },
            { typeof(BuildEventsPageViewModel), New<BuildEventsPage> },
            { typeof(BuildPageViewModel), New<BuildPage> },
            { typeof(PackagePageViewModel), New<PackagePage> },
            { typeof(SigningPageViewModel), New<SigningPage> }
        };

        public bool SupportsRecycling => false;

        public IControl Build(object param) => Templates[param.GetType()]();

        public bool Match(object data) => Templates.ContainsKey(data.GetType());

        private static T New<T>() where T : new() => new T();
    }
}
