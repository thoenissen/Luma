using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EnvDTE;
using Seth.Luma.Configuration;
using Seth.Luma.Configuration.ViewData;
using Seth.Luma.Core.Behaviors;
using Seth.Luma.Core.Helper;
using Seth.Luma.Core.ViewModel;
using Seth.Luma.Core.ViewModel.Interfaces;
using Seth.Luma.ViewData;
using VSLangProj140;

namespace Seth.Luma.ViewModel
{
    /// <summary>
    /// Edit references of loaded Projects
    /// </summary>
    public class ProjectReferencesEditorViewModel : ViewModelBase, IDialogWindowViewModel
    {
        #region Fields

        /// <summary>
        /// Projects
        /// </summary>
        private ObservableCollection<ProjectViewData> _projects;

        /// <summary>
        /// Selected project
        /// </summary>
        private ProjectViewData _selectedProject;

        /// <summary>
        /// Root directory of current Solution
        /// </summary>
        private readonly String _solutionPath;

        /// <summary>
        /// Locations
        /// </summary>
        private ObservableCollection<AssemblyLocationViewData> _assemblyLocations;

        /// <summary>
        /// Selected location
        /// </summary>
        private AssemblyLocationViewData _selectedAssemblyLocation;

        /// <summary>
        /// Commands
        /// </summary>
        private ICommand _cmdReplace;

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ProjectReferencesEditorViewModel()
        {
            _projects = new ObservableCollection<ProjectViewData>();
            
            if ((LumaPackage.Current as IServiceProvider)?.GetService(typeof(DTE)) is DTE dte)
            {
                if (dte.Solution != null)
                {
                    _solutionPath = Path.GetDirectoryName(dte.Solution.FullName);
                    
                    foreach (var project in dte.Solution.Projects.OfType<Project>())
                    {
                         ReadProjectReferences(project);
                    }
                }
            }

            var configuration = (ReferenceManagerConfiguration)LumaPackage.Current.GetDialogPage(typeof(ReferenceManagerConfiguration));

            AssemblyLocations = configuration.AssemblyLocations == null ? new ObservableCollection<AssemblyLocationViewData>() : new ObservableCollection<AssemblyLocationViewData>(configuration.AssemblyLocations.Select(obj => new AssemblyLocationViewData(obj)));
        }

        #endregion // Constructor

        #region Commands

        /// <summary>
        /// Replace
        /// </summary>
        public ICommand CmdReplace => _cmdReplace ?? (_cmdReplace = new RelayCommand(OnCmdReplace));

        /// <summary>
        /// Replace
        /// </summary>
        private void OnCmdReplace()
        {
            foreach (var project in Projects)
            {
                project.ChangeHintPath();
            }

            RaiseRequestCloseWindow();
        }

        #endregion // Commands

        #region Properties

        /// <summary>
        /// Locations
        /// </summary>
        public ObservableCollection<AssemblyLocationViewData> AssemblyLocations
        {
            get => _assemblyLocations;
            set
            {
                _assemblyLocations = value;

                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Selected location
        /// </summary>
        public AssemblyLocationViewData SelectedAssemblyLocation
        {
            get => _selectedAssemblyLocation;
            set
            {
                _selectedAssemblyLocation = value;

                RefreshNewReferencePath();

                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Projects
        /// </summary>
        public ObservableCollection<ProjectViewData> Projects
        {
            get => _projects;
            set
            {
                _projects = value;

                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Selected project
        /// </summary>
        public ProjectViewData SelectedProject
        {
            get => _selectedProject;
            set
            {
                _selectedProject = value;

                RaisePropertyChanged();
            }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Read all references of a project
        /// </summary>
        /// <param name="project"></param>
        private void ReadProjectReferences(Project project)
        {           
            if (project.Object is VSProject3)
            {
                var projectViewData = _projects.FirstOrDefault(obj => obj.Name == project.Name);

                if (projectViewData == null)
                {
                    projectViewData = new ProjectViewData(project, _solutionPath);

                    Application.Current.Dispatcher.Invoke(() => _projects.Add(projectViewData));
                }
            }
        }
        
        /// <summary>
        /// Refresh new reference path
        /// </summary>
        private void RefreshNewReferencePath()
        {
            var baseDirectory = SelectedAssemblyLocation?.Path;
            var replacedDirectory = EnvironmentHelper.ExpandEnvironmentVariables(baseDirectory);
            var directoryExists = Directory.Exists(replacedDirectory);

            foreach (var project in Projects)
            {
                foreach (var reference in project.References)
                {
                    var fileName = Path.GetFileName(reference.ReferencePath);

                    if (String.IsNullOrEmpty(replacedDirectory)
                     || String.IsNullOrEmpty(baseDirectory)
                     || String.IsNullOrEmpty(fileName)
                     || directoryExists == false)
                    {
                        reference.NewReferencePath = null;
                    }
                    else
                    {
                        if (File.Exists(Path.Combine(replacedDirectory, fileName)) == false)
                        {
                            reference.NewReferencePath = null;

                            foreach (var directory in new DirectoryInfo(replacedDirectory).GetDirectories())
                            {
                                if (File.Exists(Path.Combine(replacedDirectory, directory.Name, fileName)))
                                {
                                    reference.NewReferencePath = Path.Combine(baseDirectory, directory.Name, fileName);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            reference.NewReferencePath = Path.Combine(baseDirectory, fileName);
                        }
                    }

                }
            }
        }

        #endregion // Methods

        #region IDialogWindowViewModel

        /// <summary>
        /// Requesting to close the window
        /// </summary>
        public event RequestCloseWindowEventHandler RequestCloseWindow;

        /// <summary>
        /// Raising RequestCloseWindow
        /// </summary>
        private void RaiseRequestCloseWindow()
        {
            RequestCloseWindow?.Invoke(this, EventArgs.Empty);
        }

        #endregion // IDialogWindowViewModel
    }
}