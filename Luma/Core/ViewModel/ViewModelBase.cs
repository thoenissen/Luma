using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Seth.Luma.Core.ViewModel
{
    /// <summary>
    /// A base class for all ViewModel classes
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        #region Events

        /// <summary>
        /// Occurs when a property value changes
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

        #region IDisposable

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Internal dispose method
        /// </summary>
        /// <param name="disposing">Disposing?</param>
        protected virtual void Dispose(bool disposing)
        {   
        }

        #endregion // IDisposable
    }
}
