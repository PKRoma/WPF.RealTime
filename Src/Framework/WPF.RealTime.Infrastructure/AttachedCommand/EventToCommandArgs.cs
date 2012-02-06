using System;
using System.Windows.Input;

namespace WPF.RealTime.Infrastructure.AttachedCommand
{
    public class EventToCommandArgs
    {
        public Object Sender { get; private set; }
        public ICommand CommandRan { get; private set; }
        public Object CommandParameter { get; private set; }
        public EventArgs EventArgs { get; private set; }


        public EventToCommandArgs(Object sender, ICommand commandRan,
            Object commandParameter, EventArgs eventArgs)
        {
            Sender = sender;
            CommandRan = commandRan;
            CommandParameter = commandParameter;
            EventArgs = eventArgs;
        }
    }
}
