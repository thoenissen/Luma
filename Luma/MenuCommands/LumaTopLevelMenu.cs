using System;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Seth.Luma.Core.Data;
using Seth.Luma.Core.Window;
using Seth.Luma.ViewModel;

namespace Seth.Luma.MenuCommands
{
    /// <summary>
    /// Top level menu
    /// </summary>
    internal sealed class LumaTopLevelMenu
    {
        #region Fields

        /// <summary>
        /// VS Package
        /// </summary>
        private readonly Package _package;

        /// <summary>
        /// Reference Manager Command
        /// </summary>
        private static MenuCommand _referenceManagerCommand;

        /// <summary>
        /// Property Merger
        /// </summary>
        private MenuCommand _propertyMerger;

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="package">Owner package</param>
        private LumaTopLevelMenu(Package package)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));

            if (ServiceProvider.GetService(typeof(IMenuCommandService)) is IMenuCommandService commandService)
            {
                var solutionLoaded = ((LumaPackage.Current as IServiceProvider)?.GetService(typeof(DTE)) as DTE) ?.Solution != null;
                
                _referenceManagerCommand = new MenuCommand(ShowReferenceManager, new CommandID(LumaPackageIdentifiers.PackageGuidCmdSet, LumaPackageIdentifiers.ReferenceManagerCmdId))
                {
                    Visible = solutionLoaded
                };

                commandService.AddCommand(_referenceManagerCommand);

                _propertyMerger = new MenuCommand(ShowPropertyMerger, new CommandID(LumaPackageIdentifiers.PackageGuidCmdSet, LumaPackageIdentifiers.PropertyMergerCmdId));
                
                commandService.AddCommand(_propertyMerger);
            }

            PackageController.Current.AfterOpenSolution += OnAfterOpenSolution;
            PackageController.Current.BeforeCloseSolution += OnBeforeCloseSolution;
        }

        #endregion // Constructor

        #region Properties

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static LumaTopLevelMenu Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner _package.
        /// </summary>
        private IServiceProvider ServiceProvider => _package;

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner _package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new LumaTopLevelMenu(package);
        }

        /// <summary>
        /// Open the Reference Manager
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        private static void ShowReferenceManager(object sender, EventArgs e)
        {
            if (_referenceManagerCommand.Visible && _referenceManagerCommand.Enabled)
            {
                using (var viewModel = new ProjectReferencesEditorViewModel())
                {
                    new ViewPresenterWindow(viewModel).ShowDialog();
                }
            }
        }

        /// <summary>
        /// Open the Property Merger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowPropertyMerger(object sender, EventArgs e)
        {
            if (_referenceManagerCommand.Visible && _referenceManagerCommand.Enabled)
            {
                using (var viewModel = new PropertyMergerViewModel())
                {
                    new ViewPresenterWindow(viewModel).ShowDialog();
                }
            }
        }
        
        /// <summary>
        /// The solution is about to be closed
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private static void OnBeforeCloseSolution(Object sender, Core.Events.BeforeCloseSolutionEventArgs e)
        {
            if (_referenceManagerCommand != null)
            {
                _referenceManagerCommand.Visible = false;
            }
        }

        /// <summary>
        /// A solution was opened
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private void OnAfterOpenSolution(Object sender, Core.Events.AfterOpenSolutionEventArgs e)
        {
            if (_referenceManagerCommand != null)
            {
                _referenceManagerCommand.Visible = true;
            }
        }

        #endregion // Methods
    }
}
