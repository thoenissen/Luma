using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Seth.Luma.Configuration.Data;
using Seth.Luma.Configuration.View;
using Seth.Luma.Configuration.ViewData;
using Seth.Luma.Core.Behaviors;
using Seth.Luma.Core.Helper;

namespace Seth.Luma.Configuration
{
    /// <summary>
    /// Reference manager configuration
    /// </summary>
    public class ReferenceManagerConfiguration : UIElementDialogPage, IDataErrorInfo, INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// Visualization
        /// </summary>
        private UIElement _child;

        /// <summary>
        /// Description
        /// </summary>
        private String _description;

        /// <summary>
        /// Path
        /// </summary>
        private String _path;

        /// <summary>
        /// Preview
        /// </summary>
        private String _preview;

        /// <summary>
        /// Configuration
        /// </summary>
        private readonly SerializableReferenceManagerConfiguration _serializableConfiguration = new SerializableReferenceManagerConfiguration();

        /// <summary>
        /// Commands
        /// </summary>
        private ICommand _cmdAdd;

        #endregion // Fields

        #region Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion // Events

        #region Commands

        /// <summary>
        /// Add
        /// </summary>
        public ICommand CmdAdd => _cmdAdd ?? (_cmdAdd = new RelayCommand(OnCmdAdd));

        /// <summary>
        /// Add
        /// </summary>
        private void OnCmdAdd()
        {
            AssemblyLocations.Add(new AssemblyLocationViewData
            {
                Description = Description,
                Path = Path
            });
        }

        #endregion // Commands

        #region Properties

        /// <summary>
        /// Locations
        /// </summary>
        public ObservableCollection<AssemblyLocationViewData> AssemblyLocations { get; set; } = new ObservableCollection<AssemblyLocationViewData>();

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

        #endregion // Properties

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

        #region UIElementDialogPage

        /// <summary>
        /// Visualization
        /// </summary>
        protected override UIElement Child => _child ?? (_child = new ReferenceManagerConfigurationView { DataContext = this });

        /// <summary>
        /// Save configuration automation
        /// </summary>
        public override Object AutomationObject => _serializableConfiguration;
  
        /// <summary>
        /// Save settings to storage
        /// </summary>
        public override void SaveSettingsToStorage()
        {
            base.SaveSettingsToStorage();

            var settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
            var userSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);

            if (!userSettingsStore.CollectionExists(SettingsRegistryPath))
            {
                userSettingsStore.CreateCollection(SettingsRegistryPath);
            }

            var assemblyLocations = JsonConvert.SerializeObject(AssemblyLocations.Select(obj => new
                                                                                {
                                                                                    obj.Description,
                                                                                    obj.Path
                                                                                }));
            userSettingsStore.SetString(SettingsRegistryPath, nameof(AssemblyLocations), assemblyLocations);
        }

        /// <summary>
        /// Load setting from storage
        /// </summary>
        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();

            var settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
            var userSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);

            if (userSettingsStore.PropertyExists(SettingsRegistryPath, nameof(AssemblyLocations)))
            {
                if (JsonConvert.DeserializeObject(userSettingsStore.GetString(SettingsRegistryPath, nameof(AssemblyLocations))) is JArray assemblyLocations)
                {
                    AssemblyLocations = new ObservableCollection<AssemblyLocationViewData>(assemblyLocations.Select(obj => new AssemblyLocationViewData
                                                                                                             {
                                                                                                                 Description = obj.Value<String>("Description"),
                                                                                                                 Path = obj.Value<String>("Path"),
                                                                                                             }));
                }
                else
                {
                    AssemblyLocations = new ObservableCollection<AssemblyLocationViewData>();
                }
            }
        }

        #endregion // UIElementDialogPage

        #region IDataErrorInfo

        /// <summary>
        /// Returns the error message of the validation of the given property
        /// </summary>
        /// <param name="columnName">Property name</param>
        /// <returns>Error message</returns>
        public string this[String columnName]
        {
            get
            {
                String error = null;

                switch (columnName)
                {
                    case nameof(Description):
                        {
                            if (String.IsNullOrWhiteSpace(Description))
                            {
                                error = "Please enter a description.";
                            }
                        }
                        break;

                    case nameof(Path):
                        {
                            if (Directory.Exists(Preview) == false)
                            {
                                error = "Please enter a valid path.";
                            }
                        }
                        break;
                }

                return error;
            }
        }

        /// <summary>
        /// Not supported
        /// </summary>
        string IDataErrorInfo.Error => throw new NotSupportedException();

        #endregion // IDataErrorInfo
    }

    /// <summary>
    /// Serializable ReferenceManager configuration
    /// </summary>
    [Serializable]
    public class SerializableReferenceManagerConfiguration
    {
        /// <summary>
        /// Locations
        /// </summary>
        public SerializableAssemblyLocation[] Location;
    }
}
