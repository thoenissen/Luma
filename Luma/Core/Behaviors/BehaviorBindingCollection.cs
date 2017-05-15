using System.Windows;

namespace Seth.Luma.Core.Behaviors
{
    /// <summary>
    /// Collection to store the list of behaviors
    /// </summary>
    public class BehaviorBindingCollection : FreezableCollection<BehaviorBinding>
    {
        /// <summary>
        /// Gets or sets the Owner of the binding
        /// </summary>
        public DependencyObject Owner { get; set; }
    }
}