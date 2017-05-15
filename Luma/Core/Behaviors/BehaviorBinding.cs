using System;
using System.Windows;
using System.Windows.Input;

namespace Seth.Luma.Core.Behaviors
{
    /// <summary>
    /// Defines a Command Binding
    /// </summary>
    public sealed class BehaviorBinding : Freezable
    {
        #region Fields

        /// <summary>
        /// Stores the command behavior binding
        /// </summary>
        private CommandBehaviorBinding _behavior;

        /// <summary>
        /// Owner of the binding
        /// </summary>
        private DependencyObject _owner;
        
        #endregion // Fields

        #region Dependency properties
        
        /// <summary>
        /// Command dependency property
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(BehaviorBinding), new FrameworkPropertyMetadata(null, OnCommandChanged));

        /// <summary>
        /// Gets or sets the command property
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Handles changes to the command property
        /// </summary>
        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as BehaviorBinding)?.OnCommandChanged(e);
        }

        /// <summary>
        /// Handles changes to the command property
        /// </summary>
        private void OnCommandChanged(DependencyPropertyChangedEventArgs e)
        {
            Behavior.Command = (ICommand)e.NewValue;
        }
        
        /// <summary>
        /// CommandParameter dependency property
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(BehaviorBinding), new FrameworkPropertyMetadata((object)null, new PropertyChangedCallback(OnCommandParameterChanged)));

        /// <summary>
        /// Gets or sets the CommandParameter property.  
        /// </summary>
        public Object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        /// Handles changes to the CommandParameter property
        /// </summary>
        private static void OnCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as BehaviorBinding)?.OnCommandParameterChanged(e);
        }

        /// <summary>
        /// Handles changes to the CommandParameter property
        /// </summary>
        private void OnCommandParameterChanged(DependencyPropertyChangedEventArgs e)
        {
            Behavior.CommandParameter = e.NewValue;
        }
        
        /// <summary>
        /// Event dependency property
        /// </summary>
        public static readonly DependencyProperty EventProperty = DependencyProperty.Register(nameof(Event), typeof(String), typeof(BehaviorBinding), new FrameworkPropertyMetadata(null, OnEventChanged));

        /// <summary>
        /// Gets or sets the Event property.  
        /// </summary>
        public String Event
        {
            get => (String)GetValue(EventProperty);
            set => SetValue(EventProperty, value);
        }

        /// <summary>
        /// Handles changes to the Event property
        /// </summary>
        private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as BehaviorBinding)?.OnEventChanged(e);
        }

        /// <summary>
        /// Handles changes to the Event property
        /// </summary>
        private void OnEventChanged(DependencyPropertyChangedEventArgs e)
        {
            ResetEventBinding();
        }

        #endregion // Dependency properties

        #region Properties

        /// <summary>
        /// Stores the Command Behavior Binding
        /// </summary>
        internal CommandBehaviorBinding Behavior => _behavior ?? (_behavior = new CommandBehaviorBinding());

        /// <summary>
        /// Gets or sets the Owner of the binding
        /// </summary>
        public DependencyObject Owner
        {
            get => _owner;
            set
            {
                _owner = value;
                ResetEventBinding();
            }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Reset event binding
        /// </summary>
        private void ResetEventBinding()
        {
            if (Owner != null)
            {
                if (Behavior.Event != null && Behavior.Owner != null)
                {
                    Behavior.Dispose();
                }

                Behavior.BindEvent(Owner, Event);
            }
        }

        /// <summary>
        /// This is not actually used. This is just a trick so that this object gets WPF Inheritance Context
        /// </summary>
        /// <returns></returns>
        protected override Freezable CreateInstanceCore()
        {
            throw new NotSupportedException();
        }

        #endregion // Methods
    }
}
