using System;

namespace Seth.Luma.Core.ViewModel.Interfaces
{
    /// <summary>
    /// Requesting to close the window
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Arguments</param>
    public delegate void RequestCloseWindowEventHandler(Object sender, EventArgs e);

    /// <summary>
    /// ViewModel which is hosted in a dialog window 
    /// </summary>
    public interface IDialogWindowViewModel
    {
        /// <summary>
        /// Requesting to close the window
        /// </summary>
        event RequestCloseWindowEventHandler RequestCloseWindow;
    }
}
