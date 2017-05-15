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
        /// Reference manger command
        /// </summary>
        private static MenuCommand _referenceManagerCommand;

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
                
                _referenceManagerCommand = new MenuCommand(ShowToolWindow, new CommandID(new Guid("cd5ee33f-aef0-40c8-9d57-11836941728b"), 4130))
                {
                    Visible = solutionLoaded
                };

                commandService.AddCommand(_referenceManagerCommand);
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
        /// Shows the tool window when the menu item is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        private static void ShowToolWindow(object sender, EventArgs e)
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
