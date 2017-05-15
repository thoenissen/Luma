using System;
using System.IO;
using Microsoft.Build.Evaluation;
using Seth.Luma.Core.Helper;
using Seth.Luma.Core.ViewData;

namespace Seth.Luma.ViewData
{
    /// <summary>
    /// Information of a reference of a C#/VB project
    /// </summary>
    public class ReferenceViewData : ViewDataBase
    {
        #region Fields

        /// <summary>
        /// Name
        /// </summary>
        private String _name;
        
        /// <summary>
        /// Reference path
        /// </summary>
        private String _referencePath;

        /// <summary>
        /// New reference path
        /// </summary>
        private String _newReferencePath;

        /// <summary>
        /// Project item
        /// </summary>
        private readonly ProjectItem _projectItem;

        #endregion // Fields

        #region Contructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item"></param>
        public ReferenceViewData(ProjectItem item)
        {
            _projectItem = item;

            Name = item.UnevaluatedInclude;

            var index = Name.IndexOf(",", StringComparison.Ordinal);
            if (index != -1)
            {
                Name = Name.Remove(index);
            }

            ReferencePath = item.GetMetadata("HintPath")?.UnevaluatedValue;
        }

        #endregion // Contructor

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
        /// Reference path
        /// </summary>
        public String ReferencePath
        { 
            get => _referencePath;
            set
            {
                _referencePath = value;

                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// New reference path
        /// </summary>
        public String NewReferencePath
        {
            get => _newReferencePath;
            set
            {
                _newReferencePath = value;

                RaisePropertyChanged();
            }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Change the hint path
        /// </summary>
        public void ChangeHintPath()
        {
            if (String.IsNullOrWhiteSpace(NewReferencePath) == false)
            {
                if (File.Exists(EnvironmentHelper.ExpandEnvironmentVariables(NewReferencePath)))
                {
                    _projectItem.SetMetadataValue("HintPath", NewReferencePath);
                }
            }
        }

        #endregion // Methods
    }
}