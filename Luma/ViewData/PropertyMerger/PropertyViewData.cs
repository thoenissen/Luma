using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seth.Luma.Core.ViewData;

namespace Seth.Luma.ViewData.PropertyMerger
{
    /// <summary>
    /// Information of a property of a project
    /// </summary>
    public class PropertyViewData : ViewDataBase
    {
        #region Fields

        /// <summary>
        /// String
        /// </summary>
        private String _name;

        /// <summary>
        /// Description
        /// </summary>
        private String _description;

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="description">Description</param>
        public PropertyViewData(String name, String description)
        {
            Name = name;
            Description = description;
        }

        #endregion // Constructor

        #region Properties

        /// <summary>
        /// Name
        /// </summary>
        public String Name
        {
            get => _name;
            set
            {
                _name = value;

                RaisePropertyChanged();
            }
        }

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

        #endregion // Properties

        #region Object

        /// <summary>
        /// Return the description of the property
        /// </summary>
        /// <returns>Description</returns>
        public override String ToString()
        {
            return String.IsNullOrWhiteSpace(Description) ? Name : Description;
        }

        #endregion // Object
    }
}
