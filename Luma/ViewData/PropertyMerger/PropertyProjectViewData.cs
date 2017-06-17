using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace Seth.Luma.ViewData.PropertyMerger
{
    /// <summary>
    /// Information of a project
    /// </summary>
    public class PropertyProjectViewData : ProjectViewData
    {
        #region Fields

        /// <summary>
        /// Current value
        /// </summary>
        private Object _currentValue;
        
        #endregion // Fields
        
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="project">Project</param>
        public PropertyProjectViewData(Project project) 
            : base(project.Name)
        {
            Project = project;
        }
        
        #endregion // Constructor
        
        #region Properties

        /// <summary>
        /// Project
        /// </summary>
        public Project Project { get;set; }

        /// <summary>
        /// Current Value
        /// </summary>
        public Object CurrentValue
        {
            get => _currentValue;
            set
            {
                _currentValue = value;

                RaisePropertyChanged();
            }
        }
        
        #endregion // Properties   
    }
}
