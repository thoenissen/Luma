using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Seth.Luma.Core.ViewData
{
    /// <summary>
    /// A base class for all ViewData classes.
    /// </summary>
    public class ViewDataBase : INotifyPropertyChanged
    {
        #region Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion // Events

        #region Methods

        /// <summary>
        /// Raises PropertyChanged
        /// </summary>
        /// <param name="propertyName">Property name</param>
        protected void RaisePropertyChanged([CallerMemberName] String propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // Methods
    }
}
