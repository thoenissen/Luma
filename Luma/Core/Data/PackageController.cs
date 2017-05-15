using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Seth.Luma.Core.Events;

namespace Seth.Luma.Core.Data
{
    /// <summary>
    /// Package controller
    /// </summary>
    public class PackageController : IVsSolutionEvents3, IVsSolutionEvents4, IVsSolutionEvents5
    {
        #region Events

        /// <summary>
        /// Notifies listening clients that the project has been opened.
        /// </summary>
        public event AfterOpenProjectEventHandler AfterOpenProject;

        /// <summary>
        /// Notifies listening clients that the solution has been opened.
        /// </summary>
        public event AfterOpenSolutionEventHandler AfterOpenSolution;

        /// <summary>
        /// Notifies listening clients that the project is about to be closed
        /// </summary>
        public event BeforeCloseProjectEventHandler BeforeCloseProject;

        /// <summary>
        /// Notifies listening client that the project is about to be closed
        /// </summary>
        public event BeforeCloseSolutionEventHandler BeforeCloseSolution;
        
        #endregion // Events

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        private PackageController(IServiceProvider serviceProvider)
        {
            if (serviceProvider.GetService(typeof(SVsSolution)) is IVsSolution solution)
            {
                solution.AdviseSolutionEvents(this, out uint _);
            }
        }

        #endregion // Constructor

        #region Methods

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="serviceProvider">Service provider</param>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            Current = new PackageController(serviceProvider);
        }

        #endregion // Methods

        #region Properties

        /// <summary>
        /// Current package controller
        /// </summary>
        public static PackageController Current { get; private set; }

        #endregion // Properties

        #region IVsSolutionEvents

        /// <summary>Notifies listening clients that the project has been opened.</summary>
        /// <param name="hierarchy">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" /> interface of the project being loaded.</param>
        /// <param name="added">[in] <see langword="true" /> if the project is added to the solution after the solution is opened. <see langword="false" /> if the project is added to the solution while the solution is being opened.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        public int OnAfterOpenProject(IVsHierarchy hierarchy, int added)
        {
            AfterOpenProject?.Invoke(this, new AfterOpenProjectEventArgs(hierarchy, added));

            return VSConstants.S_OK;
        }

        /// <summary>Queries listening clients as to whether the project can be closed.</summary>
        /// <param name="hierarchy">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" /> interface of the project to be closed.</param>
        /// <param name="removing">[in] <see langword="true" /> if the project is being removed from the solution before the solution is closed. <see langword="false" /> if the project is being removed from the solution while the solution is being closed.</param>
        /// <param name="cancel">[out] <see langword="true" /> if the client vetoed the closing of the project. <see langword="false" /> if the client approved the closing of the project.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        public int OnQueryCloseProject(IVsHierarchy hierarchy, int removing, ref int cancel)
        {
            return VSConstants.S_OK;
        }

        /// <summary>Notifies listening clients that the project is about to be closed.</summary>
        /// <param name="hierarchy">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" /> interface of the project being closed.</param>
        /// <param name="removed">[in] <see langword="true" /> if the project was removed from the solution before the solution was closed. <see langword="false" /> if the project was removed from the solution while the solution was being closed.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        public int OnBeforeCloseProject(IVsHierarchy hierarchy, int removed)
        {
            BeforeCloseProject?.Invoke(this, new BeforeCloseProjectEventArgs(hierarchy, removed));

            return VSConstants.S_OK;
        }

        /// <summary>Notifies listening clients that the project has been loaded.</summary>
        /// <param name="stubHierarchy">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" /> interface of the placeholder hierarchy for the unloaded project.</param>
        /// <param name="realHierarchy">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" /> interface of the project that was loaded.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        public int OnAfterLoadProject(IVsHierarchy stubHierarchy, IVsHierarchy realHierarchy)
        {
            return VSConstants.S_OK;
        }

        /// <summary>Queries listening clients as to whether the project can be unloaded.</summary>
        /// <param name="realHierarchy">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" /> interface of the project to be unloaded.</param>
        /// <param name="cancel">[out] <see langword="true" /> if the client vetoed unloading the project. <see langword="false" /> if the client approved unloading the project.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        public int OnQueryUnloadProject(IVsHierarchy realHierarchy, ref int cancel)
        {
            return VSConstants.S_OK;
        }

        /// <summary>Notifies listening clients that the project is about to be unloaded.</summary>
        /// <param name="realHierarchy">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" /> interface of the project that will be unloaded.</param>
        /// <param name="stubHierarchy">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" /> interface of the placeholder hierarchy for the project being unloaded.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        public int OnBeforeUnloadProject(IVsHierarchy realHierarchy, IVsHierarchy stubHierarchy)
        {
            return VSConstants.S_OK;
        }

        /// <summary>Notifies listening clients that the solution has been opened.</summary>
        /// <param name="unkReserved">[in] Reserved for future use.</param>
        /// <param name="newSolution">[in] <see langword="true" /> if the solution is being created. <see langword="false" /> if the solution was created previously or is being loaded.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        public int OnAfterOpenSolution(object unkReserved, int newSolution)
        {
            AfterOpenSolution?.Invoke(this, new AfterOpenSolutionEventArgs(unkReserved, newSolution));

            return VSConstants.S_OK;
        }

        /// <summary>Queries listening clients as to whether the solution can be closed.</summary>
        /// <param name="unkReserved">[in] Reserved for future use.</param>
        /// <param name="cancel">[out] <see langword="true" /> if the client vetoed closing the solution. <see langword="false" /> if the client approved closing the solution.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        public int OnQueryCloseSolution(object unkReserved, ref int cancel)
        {
            return VSConstants.S_OK;
        }

        /// <summary>Notifies listening clients that the solution is about to be closed.</summary>
        /// <param name="unkReserved">[in] Reserved for future use.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        public int OnBeforeCloseSolution(object unkReserved)
        {
            BeforeCloseSolution?.Invoke(this, new BeforeCloseSolutionEventArgs(unkReserved));

            return VSConstants.S_OK;
        }

        /// <summary>Notifies listening clients that a solution has been closed.</summary>
        /// <param name="unkReserved">[in] Reserved for future use.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        public int OnAfterCloseSolution(object unkReserved)
        {
            return VSConstants.S_OK;
        }

        #endregion // IVsSolutionEvents

        #region IVsSolutionEvents2

        /// <summary>Queries listening clients as to whether the project can be closed.</summary>
        /// <param name="hierarchy">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" /> interface of the project to be closed.</param>
        /// <param name="removing">[in] <see langword="true" /> if the project is being removed from the solution before the solution is closed. <see langword="false" /> if the project is being removed from the solution while the solution is being closed.</param>
        /// <param name="cancel">[out] <see langword="true" /> if the client vetoed the closing of the project. <see langword="false" /> if the client approved the closing of the project.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        int IVsSolutionEvents2.OnQueryCloseProject(IVsHierarchy hierarchy, int removing, ref int cancel)
        {
            return VSConstants.S_OK;
        }

        /// <summary>Notifies listening clients that all projects have been merged into the open solution.</summary>
        /// <param name="unkReserved">[in] Reserved for future use.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        public int OnAfterMergeSolution(object unkReserved)
        {
            return VSConstants.S_OK;
        }

        #endregion // IVsSolutionEvents2

        #region IVsSolutionEvents3

        /// <summary>Fired before opening all nested projects owned by a parent hierarchy.</summary>
        /// <param name="hierarchy">[in] Pointer to parent project.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        public int OnBeforeOpeningChildren(IVsHierarchy hierarchy)
        {
            return VSConstants.S_OK;
        }

        /// <summary>Fired after opening all nested projects owned by a parent hierarchy.</summary>
        /// <param name="hierarchy">[in] Pointer to parent project.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        public int OnAfterOpeningChildren(IVsHierarchy hierarchy)
        {
            return VSConstants.S_OK;
        }

        /// <summary>Fired before closing all nested projects owned by a parent hierarchy.</summary>
        /// <param name="hierarchy">[in] Pointer to parent project.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        public int OnBeforeClosingChildren(IVsHierarchy hierarchy)
        {
            return VSConstants.S_OK;
        }

        /// <summary>Fired after closing all nested projects owned by a parent hierarchy.</summary>
        /// <param name="hierarchy">[in] Pointer to parent project.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        public int OnAfterClosingChildren(IVsHierarchy hierarchy)
        {
            return VSConstants.S_OK;
        }
        
        #endregion // IVsSolutionEvents3

        #region IVsSolutionEvents4

        /// <summary>Notifies listening clients that a project has been renamed.</summary>
        /// <param name="hierarchy">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" /> interface of the renamed project.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        int IVsSolutionEvents4.OnAfterRenameProject(IVsHierarchy hierarchy)
        {
            return VSConstants.S_OK;
        }

        /// <summary>Queries listening clients as to whether a parent project has changed.</summary>
        /// <param name="hierarchy">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" /> interface of the project parent.</param>
        /// <param name="newParentHier">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" /> interface of the changed project parent.</param>
        /// <param name="cancel">[in, out] <see langword="true" /> if the client vetoed the closing of the project. <see langword="false" /> if the client approved the closing of the project.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        int IVsSolutionEvents4.OnQueryChangeProjectParent(IVsHierarchy hierarchy, IVsHierarchy newParentHier, ref int cancel)
        {
            return VSConstants.S_OK;
        }

        /// <summary>Notifies listening clients that a project parent has changed.</summary>
        /// <param name="hierarchy">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" /> interface of the changed project parent.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        int IVsSolutionEvents4.OnAfterChangeProjectParent(IVsHierarchy hierarchy)
        {
            return VSConstants.S_OK;
        }

        /// <summary>Notifies listening clients that a project has been opened asynchronously.</summary>
        /// <param name="hierarchy">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" /> interface of the project being loaded.</param>
        /// <param name="added">[in] <see langword="true" /> if the project is added to the solution after the solution is opened. <see langword="false" /> if the project is added to the solution while the solution is being opened.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        int IVsSolutionEvents4.OnAfterAsynchOpenProject(IVsHierarchy hierarchy, int added)
        {
            return VSConstants.S_OK;
        }

        #endregion // IVsSolutionEvents4

        #region IVsSolutionEvents5

        /// <summary>Fired before each project is opened.</summary>
        /// <param name="guidProjectId">[in] The GUID of the individual project to be opened.</param>
        /// <param name="guidProjectType">[in] The GUID of the type of project (for example, Visual Basic or C#) to be opened.</param>
        /// <param name="pszFileName">[in] The name of the project file.</param>
        void IVsSolutionEvents5.OnBeforeOpenProject(ref Guid guidProjectId, ref Guid guidProjectType, string pszFileName)
        {
        }

        #endregion // IVsSolutionEvents5
    }
}
