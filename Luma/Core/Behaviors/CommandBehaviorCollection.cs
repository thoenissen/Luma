using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace Seth.Luma.Core.Behaviors
{
    /// <summary>
    /// Container for a list of behaviors
    /// </summary>
    public class CommandBehaviorCollection
    {
        #region Dependency properties

        /// <summary>
        /// Behaviors read only dependency property
        /// </summary>
        private static readonly DependencyPropertyKey BehaviorsPropertyKey = DependencyProperty.RegisterAttachedReadOnly("BehaviorsInternal", typeof(BehaviorBindingCollection), typeof(CommandBehaviorCollection), new FrameworkPropertyMetadata((BehaviorBindingCollection)null));

        /// <summary>
        /// Behaviors
        /// </summary>
        public static readonly DependencyProperty BehaviorsProperty = BehaviorsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the Behaviors property
        /// </summary>
        public static BehaviorBindingCollection GetBehaviors(DependencyObject d)
        {
            var collection = d.GetValue(BehaviorsProperty) as BehaviorBindingCollection;
            if (collection == null)
            {
                collection = new BehaviorBindingCollection
                {
                    Owner = d
                };

                SetBehaviors(d, collection);
            }
            return collection;
        }

        /// <summary>
        /// Sets the Behaviors property
        /// </summary>
        private static void SetBehaviors(DependencyObject d, BehaviorBindingCollection value)
        {
            d.SetValue(BehaviorsPropertyKey, value);

            ((INotifyCollectionChanged)value).CollectionChanged += CollectionChanged;
        }

        #endregion // Dependency properties

        #region Methods

        /// <summary>
        /// Behaviors collection changed
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private static void CollectionChanged(Object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is BehaviorBindingCollection sourceCollection)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        if (e.NewItems != null)
                        {
                            foreach (var item in e.NewItems.OfType<BehaviorBinding>())
                            {
                                item.Owner = sourceCollection.Owner;
                            }
                        }

                        break;

                    case NotifyCollectionChangedAction.Remove:
                        if (e.OldItems != null)
                        {
                            foreach (var item in e.OldItems.OfType<BehaviorBinding>())
                            {
                                item.Behavior.Dispose();
                            }
                        }

                        break;
                        
                    case NotifyCollectionChangedAction.Replace:
                        if (e.NewItems != null)
                        {
                            foreach (var item in e.NewItems.OfType<BehaviorBinding>())
                            {
                                item.Owner = sourceCollection.Owner;
                            }
                        }

                        if (e.OldItems != null)
                        {
                            foreach (var item in e.OldItems.OfType<BehaviorBinding>())
                            {
                                item.Behavior.Dispose();
                            }
                        }

                        break;
                        
                    case NotifyCollectionChangedAction.Reset:
                        if (e.OldItems != null)
                        {
                            foreach (var item in e.OldItems.OfType<BehaviorBinding>())
                            {
                                item.Behavior.Dispose();
                            }
                        }

                        break;
                }
            }
        }

        #endregion // Methods
    }
}