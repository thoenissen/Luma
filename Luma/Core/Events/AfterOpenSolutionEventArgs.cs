using System;

namespace Seth.Luma.Core.Events
{
    /// <summary>
    /// IVsSolutionEvents.OnAfterOpenSolution was raised
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Arguments</param>
    public delegate void AfterOpenSolutionEventHandler(Object sender, AfterOpenSolutionEventArgs e);

    /// <summary>
    /// IVsSolutionEvents.OnAfterOpenSolution was raised
    /// </summary>
    public class AfterOpenSolutionEventArgs : EventArgs
    {
        #region Constructor
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pUnkReserved">[in] Reserved for future use.</param>
        /// <param name="fNewSolution">[in] <see langword="true" /> if the solution is being created. <see langword="false" /> if the solution was created previously or is being loaded.</param>
        public AfterOpenSolutionEventArgs(Object pUnkReserved, int fNewSolution)
        {
            UnkReserved = pUnkReserved;
            NewSolution = fNewSolution;
        }

        #endregion // Constructor

        #region Properties

        /// <summary>
        /// Reserved for future use
        /// </summary>
        public int NewSolution { get; set; }

        /// <summary>
        /// <see langword="true" /> if the solution is being created. <see langword="false" /> if the solution was created previously or is being loaded.
        /// </summary>
        public Object UnkReserved { get; set; }

        #endregion // Properties
    }
}