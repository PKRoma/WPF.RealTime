using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace WPF.RealTime.Infrastructure.AttachedCommand
{
    /// <summary>
    /// Is a Trigger that invokes a bound Command when some 
    /// Event occurs on the attached FrameworkElement
    /// </summary>
    /// <remarks>
    /// Recommended usage:
    /// <code>
    /// IN YOUR VIEW HAVE SOMETHING LIKE THIS
    /// 
    /// 
    ///             xmlns:i="clr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity"
    ///             xmlns:CinchV2="clr-namespace:Cinch;assembly=Cinch.WPF"
    ///
    ///            <Rectangle x:Name="rectangle" >
    ///                <i:Interaction.Triggers>
    ///                    <i:EventTrigger EventName="MouseLeftButtonDown">
    ///                        <CinchV2:EventToCommandTrigger 
    ///                             Command="{Binding ViewEventToVMFiredCommand}" 
    ///                             CommandParameter="5"/>
    ///                    </i:EventTrigger>
    ///                    <i:EventTrigger EventName="MouseLeftButtonUp">
    ///                        <CinchV2:EventToCommandTrigger 
    ///                             Command="{Binding ViewEventToVMFiredCommand2}" 
    ///                             CommandParameter="{Binding ActualWidth, ElementName=rectangle}"/>
    ///                    </i:EventTrigger>
    ///                </i:Interaction.Triggers>
    ///            </Rectangle>
    ///     
    /// 
    /// AND YOU MAY HAVE SOMETHING LIKE THIS IN YOUR VIEWMODEL
    /// 
    ///         public SimpleCommand<Object,EventToCommandArgs> ViewEventToVMFiredCommand { get; private set; }
    ///         public SimpleCommand<Object, EventToCommandArgs> ViewEventToVMFiredCommand2 { get; private set; }
    ///
    ///         ViewEventToVMFiredCommand = new SimpleCommand<Object,EventToCommandArgs>(
    ///             (parameter) => { return true; },
    ///             ExecuteViewEventToVMFiredCommand);
    ///
    ///         ViewEventToVMFiredCommand2 = new SimpleCommand<Object, EventToCommandArgs>(
    ///            (parameter) => { return true; },
    ///            ExecuteViewEventToVMFiredCommand2);
    ///
    ///         private void ExecuteViewEventToVMFiredCommand(EventToCommandArgs args)
    ///         {
    ///             ICommand commandRan = args.CommandRan;
    ///             Object o = args.CommandParameter;
    ///             EventArgs ea = args.EventArgs;
    ///             var sender = args.Sender;
    ///         }
    ///
    ///         private void ExecuteViewEventToVMFiredCommand2(EventToCommandArgs args)
    ///         {
    ///             ICommand commandRan = args.CommandRan;
    ///             Object o = args.CommandParameter;
    ///             EventArgs ea = args.EventArgs;
    ///             var sender = args.Sender;
    ///         }
    ///
    /// </code>
    /// </remarks>
    public class EventToCommandTrigger : TriggerAction<FrameworkElement>
    {
        #region Overrides
        /// <param name="parameter">The EventArgs of the fired event.</param>
        protected override void Invoke(object parameter)
        {
            if (IsAssociatedElementDisabled())
            {
                return;
            }

            ICommand command = Command;
            if (command != null && command.CanExecute(CommandParameter))
            {
                command.Execute(new EventToCommandArgs(AssociatedObject, command,
                    CommandParameter, (EventArgs)parameter));
            }
        }

        /// <summary>
        /// Called when this trigger is attached to a FrameworkElement.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            EnableDisableElement();
        }
        #endregion

        #region Private Methods
        private static void OnCommandChanged(EventToCommandTrigger thisTrigger, DependencyPropertyChangedEventArgs e)
        {
            if (thisTrigger == null)
            {
                return;
            }

            if (e.OldValue != null)
            {
                ((ICommand)e.OldValue).CanExecuteChanged -= thisTrigger.OnCommandCanExecuteChanged;
            }

            ICommand command = (ICommand)e.NewValue;

            if (command != null)
            {
                command.CanExecuteChanged += thisTrigger.OnCommandCanExecuteChanged;
            }

            thisTrigger.EnableDisableElement();
        }

        private bool IsAssociatedElementDisabled()
        {
#if !SILVERLIGHT
            return AssociatedObject != null && !AssociatedObject.IsEnabled;
#else
            return false;
#endif
        }

        private void EnableDisableElement()
        {
            if (AssociatedObject == null || Command == null)
            {
                return;
            }
#if !SILVERLIGHT
            AssociatedObject.IsEnabled = Command.CanExecute(CommandParameter);
#endif

        }

        private void OnCommandCanExecuteChanged(object sender, EventArgs e)
        {
            EnableDisableElement();
        }
        #endregion

        #region DPs

        #region CommandParameter
        /// <summary>
        /// Identifies the <see cref="CommandParameter" /> dependency property
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter", typeof(object), typeof(EventToCommandTrigger),
            new PropertyMetadata(null,
                (s, e) =>
                {
                    EventToCommandTrigger sender = s as EventToCommandTrigger;
                    if (sender == null)
                    {
                        return;
                    }

                    if (sender.AssociatedObject == null)
                    {
                        return;
                    }

                    sender.EnableDisableElement();
                }));


        /// <summary>
        /// Gets or sets an object that will be passed to the <see cref="Command" />
        /// attached to this trigger. This is a DependencyProperty.
        /// </summary>
        public object CommandParameter
        {
            get { return (Object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        #endregion

        #region Command
        /// <summary
        /// >
        /// Identifies the <see cref="Command" /> dependency property
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(EventToCommandTrigger),
            new PropertyMetadata(null,
                (s, e) => OnCommandChanged(s as EventToCommandTrigger, e)));


        /// <summary>
        /// Gets or sets the ICommand that this trigger is bound to. This
        /// is a DependencyProperty.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        #endregion
        #endregion

    }

}
