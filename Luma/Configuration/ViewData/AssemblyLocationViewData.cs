using System;
using Seth.Luma.Core.Helper;
using Seth.Luma.Core.ViewData;

namespace Seth.Luma.Configuration.ViewData
{
    /// <summary>
    /// Location
    /// </summary>
    public class AssemblyLocationViewData : ViewDataBase
    {
        #region Fields

        /// <summary>
        /// Description
        /// </summary>
        private String _description;

        /// <summary>
        /// Preview
        /// </summary>
        private String _preview;

        /// <summary>
        /// Path
        /// </summary>
        private String _path;

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public AssemblyLocationViewData()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj">Object</param>
        public AssemblyLocationViewData(AssemblyLocationViewData obj)
        {
            _description = obj._description;
            _preview = obj._preview;
            _path = obj._path;
        }

        #endregion // Constructor

        #region Properties

        /// <summary>
        /// Description
        /// </summary>
        public String Description
        {
            get => _description;
            set
            {
                _description = value;
                
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Path
        /// </summary>
        public String Path
        {
            get => _path;
            set
            {
                _path = value;

                Preview = EnvironmentHelper.ExpandEnvironmentVariables(_path);

                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Preview
        /// </summary>
        public String Preview
        {
            get => _preview;
            set
            {
                _preview = value;

                RaisePropertyChanged();
            }
        }

        #endregion // Properties

        #region Object

        /// <summary>
        /// Return the description
        /// </summary>
        /// <returns>Description</returns>
        public override String ToString()
        {
            return Description;
        }

        #endregion // Object
    }
}