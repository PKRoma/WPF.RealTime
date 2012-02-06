using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using log4net;
using WPF.RealTime.Infrastructure.AttachedProperty;
using WPF.RealTime.Infrastructure.Interfaces;

namespace WPF.RealTime.Infrastructure
{
    public abstract class BaseViewModel : DispatcherObject, INotifyPropertyChanged
    {
        protected readonly ILog Log;
        protected readonly IDispatcherFacade DispatcherFacade;
 
        public FrameworkElement ViewReference { get; private set; }

        protected BaseViewModel(string view, bool vmResolvesView, bool signedMaintenanceContract)
        {
            Log = LogManager.GetLogger(view);

            // ViewModel resolves the view
            if (vmResolvesView)
            {
                var attr = (ViewAttribute)GetType().GetCustomAttributes(typeof(ViewAttribute), true).FirstOrDefault();
                if (attr == null) throw new ApplicationException("ViewAttribute is missing");

                ViewReference = (FrameworkElement)Activator.CreateInstance(attr.ViewType);

                Uri resourceLocater = new Uri("/" + attr.ViewType.Module.ScopeName.Replace(".dll", "") + ";component/Views/" +
                                                attr.ViewType.Name + ".xaml", UriKind.Relative);
                Application.LoadComponent(ViewReference, resourceLocater);
                ViewReference.DataContext = this;
            }
            // View already resolved the viewModel

            if (ViewReference is Window)
            {
                ((Window)ViewReference).Left = Convert.ToDouble(ViewReference.GetValue(WindowProperties.LeftProperty));
                ((Window)ViewReference).Top = Convert.ToDouble(ViewReference.GetValue(WindowProperties.TopProperty));
                ((Window)ViewReference).Width = Convert.ToDouble(ViewReference.GetValue(WindowProperties.WidthProperty));
                ((Window)ViewReference).Height = Convert.ToDouble(ViewReference.GetValue(WindowProperties.HeightProperty));
            }

            if (signedMaintenanceContract)
                DispatcherFacade = new MaintenanceEngineer(view, Log, Dispatcher);

        }

        public void InjectView()
        {
            var attr = (ViewAttribute)GetType().GetCustomAttributes(typeof(ViewAttribute), true).FirstOrDefault();
            if (attr == null) throw new ApplicationException("ViewAttribute is missing");

            ViewReference = (FrameworkElement)Activator.CreateInstance(attr.ViewType);

            Uri resourceLocater = new Uri("/" + attr.ViewType.Module.ScopeName.Replace(".dll", "") + ";component/Views/" +
                                            attr.ViewType.Name + ".xaml", UriKind.Relative);
            Application.LoadComponent(ViewReference, resourceLocater);
            ViewReference.DataContext = this;

            if (ViewReference is Window)
            {
                ((Window)ViewReference).Left = Convert.ToDouble(ViewReference.GetValue(WindowProperties.LeftProperty));
                ((Window)ViewReference).Top = Convert.ToDouble(ViewReference.GetValue(WindowProperties.TopProperty));
                ((Window)ViewReference).Width = Convert.ToDouble(ViewReference.GetValue(WindowProperties.WidthProperty));
                ((Window)ViewReference).Height = Convert.ToDouble(ViewReference.GetValue(WindowProperties.HeightProperty));
            }
        }

        protected T GetRef<T>(string name)
        {
            return (T)ViewReference.FindName(name);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region ProcessAsync
        protected static void ProcessAsync(Action method)
        {
            ThreadPool.QueueUserWorkItem((_) => method());
        }

        protected static void ProcessAsync<T>(Action<T> method, T parameter)
        {
            ThreadPool.QueueUserWorkItem((_) => method(parameter));
        }

        protected void ProcessOnDispatcherThread(Action method)
        {
            Dispatcher.BeginInvoke(method, null);
        }

        protected void ProcessOnDispatcherThread(Action method, DispatcherPriority priority)
        {
            Dispatcher.BeginInvoke(priority, method);
        }

        protected void ProcessOnDispatcherThread<T>(Action<T> method, T parameter)
        {
            Dispatcher.BeginInvoke(method, parameter);
        }

        protected void ProcessOnDispatcherThread<T1, T2>(Action<T1, T2> method, T1 parameter, T2 parameter2)
        {
            Dispatcher.BeginInvoke(method, parameter, parameter2);
        }

        protected void ProcessOnDispatcherThread<T>(Action<T> method, T parameter, DispatcherPriority priority)
        {
            Dispatcher.BeginInvoke(method, priority, parameter);
        }

        protected static void WithUpdateProgress(Func<Window> createUpdateWindow, Action longRunningOperation)
        {
            var dispatcherStart = new object();
            Window updateWindow = null;
            var thread = new Thread(() =>
            {
                lock (dispatcherStart)
                {
                    updateWindow = createUpdateWindow();
                    updateWindow.Closed += (sender, e) => updateWindow.Dispatcher.InvokeShutdown();
                    updateWindow.Show();
                }
                Dispatcher.Run();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            try
            {
                longRunningOperation();
            }
            finally
            {
                lock (dispatcherStart)
                {
                    updateWindow.Dispatcher.BeginInvoke((Action)(() => updateWindow.Close()));
                }
            }
        }
        #endregion
    }

}
