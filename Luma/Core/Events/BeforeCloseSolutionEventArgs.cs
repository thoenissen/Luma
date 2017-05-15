using System;

namespace Seth.Luma.Core.Events
{
    /// <summary>
    /// IVsSolutionEvents.OnBeforeCloseSolution was raised
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Arguments</param>
    public delegate void BeforeCloseSolutionEventHandler(Object sender, BeforeCloseSolutionEventArgs e);

    /// <summary>
    /// IVsSolutionEvents.OnBeforeCloseSolution was raised
    /// </summary>
    public class BeforeCloseSolutionEventArgs : EventArgs
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unkReserved">[in] Reserved for future use.</param>
        public BeforeCloseSolutionEventArgs(Object unkReserved)
        {
            UnkReserved = unkReserved;
        }

        #endregion // Constructor

        #region Properties

        /// <summary>
        /// Reserved for future use
        /// </summary>
        public Object UnkReserved { get; set; }

        #endregion // Properties
    }
}