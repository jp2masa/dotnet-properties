// Most of the code in this file is copied from Buildalyzer (https://github.com/daveaglick/Buildalyzer)

using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Utilities;

namespace DotNet.Properties
{
    internal class MSBuildProject
    {
        private PathHelper _pathHelper;
        private Dictionary<string, string> _globalProperties;
        private string _projectPath;
        private Project _project;

        public Project Project
        {
            get
            {
                if (_project == null)
                {
                    _project = Load();
                }

                return _project;
            }
        }

        public MSBuildProject(string projectPath)
        {
            _projectPath = projectPath;

            _pathHelper = new PathHelper(_projectPath);

            // Set global properties
            _globalProperties = new Dictionary<string, string>
            {
                { MSBuildProperties.SolutionDir, Path.GetDirectoryName(projectPath) },
                { MSBuildProperties.MSBuildExtensionsPath, _pathHelper.ExtensionsPath },
                { MSBuildProperties.MSBuildSDKsPath, _pathHelper.SDKsPath },
                { MSBuildProperties.RoslynTargetsPath, _pathHelper.RoslynTargetsPath },
                //{ MsBuildProperties.DesignTimeBuild, "true" },
                { MSBuildProperties.BuildProjectReferences, "false" },
                { MSBuildProperties.SkipCompilerExecution, "true" },
                { MSBuildProperties.ProvideCommandLineArgs, "true" },
                // Workaround for a problem with resource files, see https://github.com/dotnet/sdk/issues/346#issuecomment-257654120
                { MSBuildProperties.GenerateResourceMSBuildArchitecture, "CurrentArchitecture" }
            };
        }

        public Project Load()
        {
            // Create a project collection for each project since the toolset might change depending on the type of project
            ProjectCollection projectCollection = CreateProjectCollection();

            // Load the project
            using (new BuildEnvironment(_globalProperties))
            {
                return new Project(ProjectRootElement.Open(_projectPath, projectCollection, true),
                    _globalProperties, ToolLocationHelper.CurrentToolsVersion, projectCollection);
            }
        }

        private ProjectCollection CreateProjectCollection()
        {
            ProjectCollection projectCollection = new ProjectCollection(_globalProperties);

            projectCollection.RemoveAllToolsets();  // Make sure we're only using the latest tools
            projectCollection.AddToolset(new Toolset(ToolLocationHelper.CurrentToolsVersion, _pathHelper.ToolsPath, projectCollection, string.Empty));
            projectCollection.DefaultToolsVersion = ToolLocationHelper.CurrentToolsVersion;

            return projectCollection;
        }
    }
}
