using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Seth.Luma.Configuration;
using Seth.Luma.Core.Data;
using Seth.Luma.MenuCommands;

namespace Seth.Luma
{
    /// <summary>
    /// Luma package
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("Luma", "Your little helper Luma.", "1.0.0.3", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid("0ea1c363-649f-47e4-b230-bccc3fd7e9dc")]
    [ProvideOptionPage(typeof(ReferenceManagerConfiguration), "Luma", "Reference manager", 0, 0, true)]
    [ProvideAutoLoad("F1536EF8-92EC-443C-9ED7-FDADF150DA82 ")]
    public sealed class LumaPackage : Package
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LumaPackage()
        {
            Current = this;
        }

        /// <summary>
        /// Initialization of the package
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            PackageController.Initialize(this);
            LumaTopLevelMenu.Initialize(this);
        }

        /// <summary>
        /// Current instance
        /// </summary>
        public static LumaPackage Current
        {
            get;
            private set;
        }
    }
}
