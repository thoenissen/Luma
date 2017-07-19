using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using EnvDTE;
using Seth.Luma.Core.Behaviors;
using Seth.Luma.Core.ViewModel;
using Seth.Luma.Helper;
using Seth.Luma.ViewData.PropertyMerger;
using VSLangProj;
using VSLangProj140;

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
        private ObservableCollection<PropertyViewData> _properties;

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
        private PropertyViewData _selectedProperty;
        /// <summary>
        /// New values
        /// </summary>
        private ObservableCollection<object> _newValues;

        /// <summary>
        /// New values
        /// </summary>
        private Object _selectedNewValue;

        /// <summary>
        /// Default values
        /// </summary>
        private static readonly Dictionary<String, ObservableCollection<Object>> DefaultValues = new Dictionary<String, ObservableCollection<Object>>()
        {
            ["RunCodeAnalysis"] = new ObservableCollection<Object> { true, false },
            ["NoStdLib"] = new ObservableCollection<Object> { true, false },
            ["CodeAnalysisUseTypeNameInSuppression"] = new ObservableCollection<Object> { true, false },
            ["Optimize"] = new ObservableCollection<Object> { true, false },
            ["TreatWarningsAsErrors"] = new ObservableCollection<Object> { true, false },
            ["EnableASPDebugging"] = new ObservableCollection<Object> { true, false },
            ["IncrementalBuild"] = new ObservableCollection<Object> { true, false },
            ["CodeAnalysisFailOnMissingRules"] = new ObservableCollection<Object> { true, false },
            ["UseVSHostingProcess"] = new ObservableCollection<Object> { true, false },
            ["DefineDebug"] = new ObservableCollection<Object> { true, false },
            ["CodeAnalysisIgnoreBuiltInRules"] = new ObservableCollection<Object> { true, false },
            ["CodeAnalysisOverrideRuleVisibilities"] = new ObservableCollection<Object> { true, false },
            ["DefineTrace"] = new ObservableCollection<Object> { true, false },
            ["DebugSymbols"] = new ObservableCollection<Object> { true, false },
            ["CodeAnalysisIgnoreBuiltInRuleSets"] = new ObservableCollection<Object> { true, false },
            ["CodeAnalysisIgnoreGeneratedCode"] = new ObservableCollection<Object> { true, false },
            ["EnableSQLServerDebugging"] = new ObservableCollection<Object> { true, false },
            ["RemoteDebugEnabled"] = new ObservableCollection<Object> { true, false },
            ["AllowUnsafeBlocks"] = new ObservableCollection<Object> { true, false },
            ["EnableUnmanagedDebugging"] = new ObservableCollection<Object> { true, false },
            ["StartWithIE"] = new ObservableCollection<Object> { true, false },
            ["CheckForOverflowUnderflow"] = new ObservableCollection<Object> { true, false },
            ["Prefer32Bit"] = new ObservableCollection<Object> { true, false },
            ["RegisterForComInterop"] = new ObservableCollection<Object> { true, false },
            ["EnableASPXDebugging"] = new ObservableCollection<Object> { true, false },
            ["RemoveIntegerChecks"] = new ObservableCollection<Object> { true, false },

            ["LanguageVersion"] = new ObservableCollection<Object> { "default", "ISO-1", "ISO-2", "3", "4", "5", "6", "7"},
            ["PlatformTarget"] = new ObservableCollection<Object> { "Any CPU", "x86", "x64"},
            ["DebugInfo"] = new ObservableCollection<Object> { "full", "pdbonly", "none", "embedded", "portable" },

            ["FileAlignment"] = new ObservableCollection<Object> { 512u, 1024u, 2048u, 4096u, 8192u },

            ["WarningLevel"] = new ObservableCollection<Object> { 0, 1, 2, 3, 4 },
        };

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
            Properties = new ObservableCollection<PropertyViewData>();
            Projects = new ObservableCollection<PropertyProjectViewData>();

            Properties.Add(new PropertyViewData("RunCodeAnalysis", "Code Analysis: Enable on Build"));
            Properties.Add(new PropertyViewData("NoStdLib", "Do not reference mscorlib.dll"));
            Properties.Add(new PropertyViewData("CodeAnalysisUseTypeNameInSuppression", "Code Analysis: Use type name in suppression"));
            Properties.Add(new PropertyViewData("Optimize", "Optimize code"));
            Properties.Add(new PropertyViewData("TreatWarningsAsErrors", "Treat Warnings as error"));
            Properties.Add(new PropertyViewData("EnableASPDebugging", "Enable debugging of Active Server Pages"));
            Properties.Add(new PropertyViewData("IncrementalBuild", "Incremental Build"));
            Properties.Add(new PropertyViewData("CodeAnalysisFailOnMissingRules", "Code Analysis: Fails on missing rule set"));
            Properties.Add(new PropertyViewData("UseVSHostingProcess", "Use VS Hosting Process"));
            Properties.Add(new PropertyViewData("DefineDebug", "Define DEBUG constant"));
            Properties.Add(new PropertyViewData("CodeAnalysisIgnoreBuiltInRules", "Code Analysis: Ignore build in rules"));
            Properties.Add(new PropertyViewData("CodeAnalysisOverrideRuleVisibilities", "Code Analysis: Override rule visibilities"));
            Properties.Add(new PropertyViewData("CodeAnalysisIgnoreBuiltInRuleSets", "Code Analysis: Ignore built in rule sets"));
            Properties.Add(new PropertyViewData("CodeAnalysisIgnoreGeneratedCode", "Code Analysis: Suppress results from generated code (managed only)"));
            Properties.Add(new PropertyViewData("DefineTrace", "Define TRACE constant"));
            Properties.Add(new PropertyViewData("DebugSymbols", "Debug Symbols"));
            Properties.Add(new PropertyViewData("EnableSQLServerDebugging", "Enable SQL Server debugging"));
            Properties.Add(new PropertyViewData("RemoteDebugEnabled", "Use remote machine"));
            Properties.Add(new PropertyViewData("AllowUnsafeBlocks", "Allow unsafe code"));
            Properties.Add(new PropertyViewData("EnableUnmanagedDebugging", "Enable native code debugging"));
            Properties.Add(new PropertyViewData("StartWithIE", "Start with IE"));
            Properties.Add(new PropertyViewData("CheckForOverflowUnderflow", "Check for arithmetic overflow/underflow"));
            Properties.Add(new PropertyViewData("Prefer32Bit", "Prefer 32-bit"));
            Properties.Add(new PropertyViewData("RegisterForComInterop", "Register for COM interop"));
            Properties.Add(new PropertyViewData("EnableASPXDebugging", "Enable ASPX debugging"));
            Properties.Add(new PropertyViewData("RemoveIntegerChecks", "Remove Integer checks"));
            Properties.Add(new PropertyViewData("LanguageVersion", "Language Version"));
            Properties.Add(new PropertyViewData("PlatformTarget", "Platform Target"));
            Properties.Add(new PropertyViewData("DebugInfo", "Debug Info"));
            Properties.Add(new PropertyViewData("FileAlignment", "File Alignment"));
            Properties.Add(new PropertyViewData("BaseAddress", "Base Address"));
            Properties.Add(new PropertyViewData("StartURL", "Start URL"));
            Properties.Add(new PropertyViewData("TreatSpecificWarningsAsErrors", "Treat specific warnings as errors"));
            Properties.Add(new PropertyViewData("StartArguments", "Start Arguments"));
            Properties.Add(new PropertyViewData("IntermediatePath", "Intermediate Path"));
            Properties.Add(new PropertyViewData("CodeAnalysisRuleDirectories", "Code Analysis: Rule Directories"));
            Properties.Add(new PropertyViewData("RemoteDebugMachine", ""));
            Properties.Add(new PropertyViewData("CodeAnalysisSpellCheckLanguages", "Code Analysis: Spell check languages"));
            Properties.Add(new PropertyViewData("CodeAnalysisRules", "Code Analysis: Rules"));
            Properties.Add(new PropertyViewData("StartAction", "Start Action"));
            Properties.Add(new PropertyViewData("ConfigurationOverrideFile", "Configuration Override File"));
            Properties.Add(new PropertyViewData("WarningLevel", " Warning level"));
            Properties.Add(new PropertyViewData("ErrorReport", "Error Report"));
            Properties.Add(new PropertyViewData("CodeAnalysisInputAssembly", "Code Analysis: Input Assembly"));
            Properties.Add(new PropertyViewData("CodeAnalysisDictionaries", "Code Analysis: Dictionaries"));
            Properties.Add(new PropertyViewData("GenerateSerializationAssemblies", "Generate Serialization Assemblies"));
            Properties.Add(new PropertyViewData("CodeAnalysisModuleSuppressionsFile", "Code Analysis: Module Suppression File"));
            Properties.Add(new PropertyViewData("StartWorkingDirectory", "Start up working directory"));
            Properties.Add(new PropertyViewData("DocumentationFile", "Documentation File"));
            Properties.Add(new PropertyViewData("StartPage", "Start Page"));
            Properties.Add(new PropertyViewData("OutputPath", "Output Path"));
            Properties.Add(new PropertyViewData("CodeAnalysisLogFile", "Code Analysis: Log File"));
            Properties.Add(new PropertyViewData("DefineConstants", "Constants"));
            Properties.Add(new PropertyViewData("StartProgram", "Start Program"));
            Properties.Add(new PropertyViewData("CodeAnalysisRuleSetDirectories", "Code Analysis: Rule Set Directory"));
            Properties.Add(new PropertyViewData("CodeAnalysisCulture", "Code Analysis: Culture"));
            Properties.Add(new PropertyViewData("CodeAnalysisRuleAssemblies", "Code Analysis: Rule Assemblies"));
            Properties.Add(new PropertyViewData("CodeAnalysisRuleSet", "Code Analysis: Rule Set"));
            Properties.Add(new PropertyViewData("NoWarn", "Suppress warnings"));

            if ((LumaPackage.Current as IServiceProvider)?.GetService(typeof(DTE)) is DTE dte)
            {
                if (dte.Solution != null)
                {
                    foreach (var project in SolutionHelper.GetAllProjects(dte.Solution).Where(obj => obj.Object is VSProject3))
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
                            if (Properties.FirstOrDefault(obj => obj.Name == property.Name) == null)
                            {
                                Properties.Add(new PropertyViewData(property.Name, null));
                            }
                        }

                        Projects.Add(new PropertyProjectViewData(project));
                    }
                }
            }

            Properties = new ObservableCollection<PropertyViewData>(Properties.OrderBy(obj => obj.ToString()));

            SelectedPlatformName = PlatformNames.FirstOrDefault();
            SelectedConfiguration = Configurations.FirstOrDefault();
            SelectedProperty = Properties.FirstOrDefault();
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
                            configuration.Properties.Item(SelectedProperty.Name).Value = SelectedNewValue;
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
        public ObservableCollection<PropertyViewData> Properties
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
        public PropertyViewData SelectedProperty
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
            if (SelectedProperty != null)
            {
                var addValues = true;

                if (DefaultValues.ContainsKey(SelectedProperty.Name))
                {
                    NewValues = DefaultValues[SelectedProperty.Name];

                    addValues = false;
                }
                else
                {
                    NewValues = new ObservableCollection<Object>();
                }
                
                foreach (var project in Projects)
                {
                    object currentValue = null;

                    if (SelectedPlatformName != null
                        && SelectedConfiguration != null)
                    {
                        foreach (var configuration in project.Project.ConfigurationManager
                            .Platform(SelectedPlatformName).OfType<EnvDTE.Configuration>())
                        {
                            if (configuration.ConfigurationName == SelectedConfiguration)
                            {
                                currentValue = configuration.Properties.Item(SelectedProperty.Name).Value;

                                if (addValues && NewValues.Contains(currentValue) == false)
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
        }

        #endregion // Methods   
    }
}
