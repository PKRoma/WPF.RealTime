using System.Windows;
using System.Windows.Threading;

namespace WPF.RealTime.Infrastructure.Utils
{
    public static class ApplicationExtensions
    {
        #region DoEvents using a DispatcherFrame

        public static void DoEvents(this Application application)
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new ExitFrameHandler(frm => frm.Continue = false), frame);
            Dispatcher.PushFrame(frame);
        }

        private delegate void ExitFrameHandler(DispatcherFrame frame);

        #endregion

        #region DoEvents without using a DispatcherFrame

        //public static void DoEvents(this Application application)
        //{
        //    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, new VoidHandler(() => { }));
        //}

        //private delegate void VoidHandler();

        #endregion
    }
}
