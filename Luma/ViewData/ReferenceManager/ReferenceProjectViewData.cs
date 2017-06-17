using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Build.Evaluation;

namespace Seth.Luma.ViewData.ReferenceManager
{
    /// <summary>
    /// Information of a project
    /// </summary>
    public class ReferenceProjectViewData : ProjectViewData
    {
        #region Fields

        /// <summary>
        /// References
        /// </summary>
        private ObservableCollection<ReferenceViewData> _references;

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dteProject">Project</param>
        /// <param name="solutionPath">Path to the root of the solution</param>
        public ReferenceProjectViewData(EnvDTE.Project dteProject, String solutionPath)
            : base (dteProject.Name)
        {
            References = new ObservableCollection<ReferenceViewData>();

            Application.Current.Dispatcher.Invoke(() => References.Clear());

            var buildProject = ProjectCollection.GlobalProjectCollection.GetLoadedProjects(dteProject.FullName).First();

            foreach (var item in buildProject.Items.Where(obj => obj.ItemType == "Reference"))
            {
                if (item.HasMetadata("HintPath"))
                {
                    var hintPath = item.GetMetadata("HintPath").UnevaluatedValue;

                    while (hintPath.StartsWith("..\\"))
                    {
                        hintPath = hintPath.Remove(0, 3);
                    }

                    // Filter NuGet references
                    if (hintPath.StartsWith(Path.Combine(solutionPath, "packages")) == false
                     && hintPath.StartsWith("packages") == false)
                    {
                        Application.Current.Dispatcher.Invoke(() => References.Add(new ReferenceViewData(item)));
                    }
                }
            }
        }

        #endregion // Constructor

        #region Properties

        /// <summary>
        /// References
        /// </summary>
        public ObservableCollection<ReferenceViewData> References
        {
            get => _references;
            set
            {
                _references = value;

                RaisePropertyChanged();
            }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Changing the hint path
        /// </summary>
        public void ChangeHintPath()
        {
            foreach (var reference in References)
            {
                reference.ChangeHintPath();
            }
        }

        #endregion // Methods
    }
}
