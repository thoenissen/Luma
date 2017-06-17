using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EnvDTE;
using Seth.Luma.Configuration;
using Seth.Luma.Configuration.ViewData;
using Seth.Luma.Core.Behaviors;
using Seth.Luma.Core.ViewModel;
using Seth.Luma.ViewData;
using Seth.Luma.Helper;
using Seth.Luma.ViewData.PropertyMerger;
using VSLangProj;

namespace Seth.Luma.ViewModel
{
    /// <summary>
    /// Property Merger
    /// </summary>
    public class PropertyMergerViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// Platform names
        /// </summary>
        private ObservableCollection<String> _platformNames;

        /// <summary>
        /// Configuration
        /// </summary>
        private ObservableCollection<String> _configurations;

        /// <summary>
        /// Properties
        /// </summary>
        private ObservableCollection<String> _properties;

        /// <summary>
        /// Projects
        /// </summary>
        private ObservableCollection<PropertyProjectViewData> _projects;

        /// <summary>
        /// Selected platform
        /// </summary>
        private String _selectedPlatformName;

        /// <summary>
        /// Selected Configuration
        /// </summary>
        private String _selectedConfiguration;

        /// <summary>
        /// Selected property
        /// </summary>
        private String _selectedProperty;
        /// <summary>
        /// New values
        /// </summary>
        private ObservableCollection<object> _newValues;

        /// <summary>
        /// New values
        /// </summary>
        private Object _selectedNewValue;

        /// <summary>
        /// Commands
        /// </summary>
        private ICommand _cmdMerge;

        #endregion // Fields
        
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public PropertyMergerViewModel()
        {
            PlatformNames = new ObservableCollection<String>();
            Configurations = new ObservableCollection<String>();
            Properties = new ObservableCollection<String>();
            Projects = new ObservableCollection<PropertyProjectViewData>();

            if ((LumaPackage.Current as IServiceProvider)?.GetService(typeof(DTE)) is DTE dte)
            {
                if (dte.Solution != null)
                {
                    foreach (var project in SolutionHelper.GetAllProjects(dte.Solution).Where(obj => obj.Object is VSProject))
                    {
                        foreach (var platformName in (Object[])project.ConfigurationManager.PlatformNames)
                        {
                            if (PlatformNames.Contains(platformName.ToString()) == false)
                            {
                                PlatformNames.Add(platformName.ToString());
                            }
                        }

                        foreach (var configurationName in (Object[])project.ConfigurationManager.ConfigurationRowNames)
                        {
                            if (Configurations.Contains(configurationName.ToString()) == false)
                            {
                                Configurations.Add(configurationName.ToString());
                            }
                        }

                        foreach (var property in project.ConfigurationManager.ActiveConfiguration.Properties.OfType<Property>())
                        {
                            if (Properties.Contains(property.Name) == false)
                            {
                                Properties.Add(property.Name);
                            }
                        }

                        Projects.Add(new PropertyProjectViewData(project));
                    }
                }
            }
        }

        #endregion // Constructor

        /// <summary>
        /// Merge
        /// </summary>
        public ICommand CmdMerge => _cmdMerge ?? (_cmdMerge = new RelayCommand(OnCmdMerge));

        /// <summary>
        /// Merge
        /// </summary>
        private void OnCmdMerge()
        {
            foreach (var project in Projects.Where(obj => obj.IsSelected))
            {
                if (SelectedPlatformName != null
                    && SelectedProperty != null
                    && SelectedConfiguration != null)
                {
                    foreach (var configuration in project.Project.ConfigurationManager.Platform(SelectedPlatformName).OfType<EnvDTE.Configuration>())
                    {
                        if (configuration.ConfigurationName == SelectedConfiguration)
                        {
                            configuration.Properties.Item(SelectedProperty).Value = SelectedNewValue;
                            break;
                        }
                    }
                }
            }

            Refresh();
        }

        #region Properties

        /// <summary>
        /// Platform names
        /// </summary>
        public ObservableCollection<String> PlatformNames
        {
            get => _platformNames;
            set
            {
                _platformNames = value;

                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Selected platform
        /// </summary>
        public String SelectedPlatformName
        {
            get => _selectedPlatformName;
            set
            {
                _selectedPlatformName = value;

                Refresh();

                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Configurations
        /// </summary>
        public ObservableCollection<String> Configurations
        {
            get => _configurations;
            set
            {
                _configurations = value;

                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Selected configuration
        /// </summary>
        public String SelectedConfiguration
        {
            get => _selectedConfiguration;
            set
            {
                _selectedConfiguration = value;

                Refresh();

                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Properties
        /// </summary>
        public ObservableCollection<String> Properties
        {
            get => _properties;
            set
            {
                _properties = value;

                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Selected property
        /// </summary>
        public String SelectedProperty
        {
            get => _selectedProperty;
            set
            {
                _selectedProperty = value;

                Refresh();

                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Projects
        /// </summary>
        public ObservableCollection<PropertyProjectViewData> Projects
        {
            get => _projects;
            set
            {
                _projects = value;

                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// New values
        /// </summary>
        public ObservableCollection<Object> NewValues
        {
            get => _newValues;
            set
            {
                _newValues = value;

                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Selected new values
        /// </summary>
        public Object SelectedNewValue
        {
            get => _selectedNewValue;
            set
            {
                _selectedNewValue = value;

                RaisePropertyChanged();
            }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Refresh
        /// </summary>
        private void Refresh()
        {
            NewValues = new ObservableCollection<Object>();

            foreach (var project in Projects)
            {
                object currentValue = null;

                if (SelectedPlatformName != null
                 && SelectedProperty != null
                 && SelectedConfiguration != null)
                {
                    foreach (var configuration in project.Project.ConfigurationManager.Platform(SelectedPlatformName).OfType<EnvDTE.Configuration>())
                    {
                        if (configuration.ConfigurationName == SelectedConfiguration)
                        {
                            currentValue = configuration.Properties.Item(SelectedProperty).Value;

                            if (NewValues.Contains(currentValue) == false)
                            {
                                NewValues.Add(currentValue);
                            }

                            break;
                        }
                    }
                }

                project.CurrentValue = currentValue;
            }
        }

        #endregion // Methods   
    }
}
