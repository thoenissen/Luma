using System;
using Microsoft.VisualStudio.Shell.Interop;

namespace Seth.Luma.Core.Events
{
    /// <summary>
    /// IVsSolutionEvents.OnBeforeCloseProject was raised
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Arguments</param>
    public delegate void BeforeCloseProjectEventHandler(Object sender, BeforeCloseProjectEventArgs e);

    /// <summary>
    /// IVsSolutionEvents.OnBeforeCloseProject was raised
    /// </summary>
    public class BeforeCloseProjectEventArgs : EventArgs
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="project">Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" /> interface of the project being loaded.</param>
        /// <param name="removed"><see langword="true" /> if the project was removed from the solution before the solution was closed. <see langword="false" /> if the project was removed from the solution while the solution was being closed.</param>
        public BeforeCloseProjectEventArgs(IVsHierarchy project, int removed)
        {
            Project = project;
            Removed = removed;
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
        public int Removed { get; }

        #endregion // Properties
    }
}
