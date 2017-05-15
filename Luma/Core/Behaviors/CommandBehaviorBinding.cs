using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Input;

namespace Seth.Luma.Core.Behaviors
{
    /// <summary>
    /// Defines the command behavior binding
    /// </summary>
    public sealed class CommandBehaviorBinding : IDisposable
    {
        #region Fields

        /// <summary>
        /// IDisposable
        /// </summary>
        private bool _disposed;
        
        #endregion // Fields
        
        #region Properties

        /// <summary>
        /// Get the owner of the CommandBinding
        /// </summary>
        public DependencyObject Owner { get; private set; }

        /// <summary>
        /// The event name to hook up to
        /// </summary>
        public string EventName { get; private set; }

        /// <summary>
        /// The event info of the event
        /// </summary>
        public EventInfo Event { get; private set; }

        /// <summary>
        /// Gets the EventHandler for the binding with the event
        /// </summary>
        public Delegate EventHandler { get; private set; }
        
        /// <summary>
        /// Gets or sets a CommandParameter
        /// </summary>
        public object CommandParameter { get; set; }

        /// <summary>
        /// The command to execute when the specified event is raised
        /// </summary>
        public ICommand Command { get; set; }
        
        #endregion // Properties

        #region Methods

        /// <summary>
        /// Bind the event
        /// </summary>
        /// <param name="owner">Owner</param>
        /// <param name="eventName">Event name</param>
        public void BindEvent(DependencyObject owner, string eventName)
        {
            EventName = eventName;
            Owner = owner;

            Event = Owner.GetType().GetEvent(EventName, BindingFlags.Public | BindingFlags.Instance);

            if (Event != null)
            {
                EventHandler = CreateDelegate();
                
                Event.AddEventHandler(Owner, EventHandler);
            }
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        public void Execute()
        {
            Command?.Execute(CommandParameter);
        }

        /// <summary>
        /// Generates a delegate with a matching signature of  Event.EventHandlerType.GetMethod("Invoke")
        /// </summary>
        /// <returns>Returns a delegate with the same signature as Event.EventHandlerType.GetMethod("Invoke")</returns>
        public Delegate CreateDelegate()
        {
            // Parameters of the event method
            var delegateParameters = Event.EventHandlerType.GetMethod("Invoke").GetParameters();
            var hookupParameters = new Type[delegateParameters.Length + 1];
            hookupParameters[0] = GetType();
            for (var i = 0; i < delegateParameters.Length; i++)
            {
                hookupParameters[i + 1] = delegateParameters[i].ParameterType;
            }

            // Dynamic method to handle the event
            var handler = new DynamicMethod("", null, hookupParameters, GetType());

            var eventIl = handler.GetILGenerator();
            
            var local = eventIl.DeclareLocal(typeof(object[]));
            eventIl.Emit(OpCodes.Ldc_I4, delegateParameters.Length + 1);
            eventIl.Emit(OpCodes.Newarr, typeof(object));
            eventIl.Emit(OpCodes.Stloc, local);
            
            for (var i = 1; i < delegateParameters.Length + 1; i++)
            {
                eventIl.Emit(OpCodes.Ldloc, local);
                eventIl.Emit(OpCodes.Ldc_I4, i);
                eventIl.Emit(OpCodes.Ldarg, i);
                eventIl.Emit(OpCodes.Stelem_Ref);
            }

            eventIl.Emit(OpCodes.Ldloc, local);
            eventIl.Emit(OpCodes.Ldarg_0);
            eventIl.EmitCall(OpCodes.Call, GetType().GetMethod(nameof(Execute), BindingFlags.Public | BindingFlags.Instance), null);

            eventIl.Emit(OpCodes.Pop);
            eventIl.Emit(OpCodes.Ret);
            
            return handler.CreateDelegate(Event.EventHandlerType, this);
        }

        #endregion // Methods

        #region IDisposable

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                Event.RemoveEventHandler(Owner, EventHandler);
                _disposed = true;
            }
        }

        #endregion // IDisposable
    }
}
