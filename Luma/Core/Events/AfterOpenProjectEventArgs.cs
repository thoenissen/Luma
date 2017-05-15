using System;
using Microsoft.VisualStudio.Shell.Interop;

namespace Seth.Luma.Core.Events
{
    /// <summary>
    /// IVsSolutionEvents.OnAfterOpenProject was raised
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Arguments</param>
    public delegate void AfterOpenProjectEventHandler(Object sender, AfterOpenProjectEventArgs e);

    /// <summary>
    /// IVsSolutionEvents.OnAfterOpenProject was raised
    /// </summary>
    public class AfterOpenProjectEventArgs : EventArgs
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="project">Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" /> interface of the project being loaded.</param>
        /// <param name="added"><see langword="true" /> if the project is added to the solution after the solution is opened. <see langword="false" /> if the project is added to the solution while the solution is being opened.</param>
        public AfterOpenProjectEventArgs(IVsHierarchy project, int added)
        {
            Project = project;
            Added = added;
        }
        
        #endregion // Constructor

        #region Properties

        /// <summary>
        /// Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" /> interface of the project being loaded.
        /// </summary>
        public IVsHierarchy Project { get; }

        /// <summary>
        /// <see langword="true" /> if the project is added to the solution after the solution is opened. <see langword="false" /> if the project is added to the solution while the solution is being opened.
        /// </summary>
        public int Added { get; }

        #endregion // Properties
    }
}