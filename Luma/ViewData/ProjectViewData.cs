using System;
using Seth.Luma.Core.ViewData;

namespace Seth.Luma.ViewData
{
    /// <summary>
    /// Information of a project
    /// </summary>
    public class ProjectViewData : ViewDataBase
    {
        #region Fields

        /// <summary>
        /// Is selected?
        /// </summary>
        private bool _isSelected;
        
        #endregion // Fields
        
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="projectName">Project name</param>
        public ProjectViewData(String projectName)
        {
            Name = projectName;
            IsSelected = true;
        }

        #endregion // Constructor

        #region Properties

        /// <summary>
        /// Project name
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Is selected?
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;

                RaisePropertyChanged();
            }
        }

        #endregion // Properties
    }
}
