using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using log4net;
using WPF.RealTime.Data;
using WPF.RealTime.Data.ResourceManager;
using WPF.RealTime.Infrastructure;
using WPF.RealTime.Infrastructure.Interfaces;
using WPF.RealTime.Infrastructure.Messaging;

namespace WPF.RealTime.Shell
{
    public sealed class Bootstrapper
    {
        // design time imports
        [ImportMany(typeof(IStaticViewModel))]
        public IEnumerable<IStaticViewModel> StaticViewModels;

        [ImportMany(typeof(IDynamicViewModel))]
        public IEnumerable<Lazy<IDynamicViewModel>> DynamicViewModels;

        [ImportMany(typeof(BaseServiceObserver))]
        public IEnumerable<Lazy<BaseServiceObserver>> ServiceObservers;

        // run-time imports
        public IEnumerable<Lazy<IService>> Services;

        public void Run()
        {
            DiscoverParts();
            _log.Info("Parts discovered");

            MainWindow mainWindow = new MainWindow();
            InjectStaticViewModels(mainWindow);
            _log.Info("Static View Models injected");
            mainWindow.Show();

            Application.Current.MainWindow = mainWindow;

            InjectDynamicViewModels(_dm == "MULTI");
            _log.Info("Dynamic View Models injected");
            InjectServices();
            _log.Info("Services injected");
        }

        private readonly ILog _log = LogManager.GetLogger(typeof(Bootstrapper));
        private CompositionContainer _container;
        private static readonly ConcurrentDictionary<string, Window> RunningWindows = new ConcurrentDictionary<string, Window>();
        private readonly string _sm = Convert.ToString(ConfigurationManager.AppSettings["SERVICE_MODE"]);
        private readonly string _dm = Convert.ToString(ConfigurationManager.AppSettings["DISPATCHER_MODE"]);
        private readonly string _sp = Convert.ToString(ConfigurationManager.AppSettings["SERVICES_PATH"]);
        private readonly string _mp = Convert.ToString(ConfigurationManager.AppSettings["MODULES_PATH"]);

        public Bootstrapper()
        {
            Mediator.GetInstance.RegisterInterest<Lazy<IDynamicViewModel>>(Topic.BootstrapperLoadViews, _createView, TaskType.LongRunning);
            Mediator.GetInstance.RegisterInterest<string>(Topic.BootstrapperUnloadView, UnloadView, TaskType.LongRunning);

        }

        private void DiscoverParts()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(_mp));
            catalog.Catalogs.Add(new DirectoryCatalog(_sp));

            //Create the CompositionContainer with the parts in the catalog
            _container = new CompositionContainer(catalog);
            
            //Fill the imports of this object)
            try
            {
                _log.Info(String.Format("{0} Mode", _sm));
                _container.ComposeParts(this);
                Services = _container.GetExports<IService>(_sm);
            }
            catch (CompositionException compositionException)
            {
                throw new ApplicationException("Opps", compositionException);
            }
 
        }

        private static void UnloadView(string key)
        {
            Window view;
            if (RunningWindows.TryGetValue(key, out view))
            {
                Action close = () => view.Close();
                view.Dispatcher.BeginInvoke(close);
                Window removed;
                RunningWindows.TryRemove(key, out removed);
                view.Dispatcher.BeginInvoke((Action)(() => Mediator.GetInstance.Unregister(removed.DataContext)));
                var heartbeat = new Heartbeat(removed.GetType().ToString(), String.Format("{0} View unloaded at: {1}", removed.GetType(), DateTime.UtcNow.ToLongTimeString()), DateTime.UtcNow, true);

                Mediator.GetInstance.Broadcast(Topic.ShellStateUpdated, heartbeat);
            }
        }

        private static void CreateView(Lazy<IDynamicViewModel> lazy)
        {
            var vm = lazy.Value;

            Mediator.GetInstance.Register(vm);
            Window view = (Window)((BaseViewModel)vm).ViewReference;
            RunningWindows.TryAdd(vm.DynamicViewName, view);
            var heartbeat = new Heartbeat(vm.GetType().ToString(), String.Format("{0} View loaded at: {1}", vm.GetType().ToString(), DateTime.UtcNow.ToLongTimeString()), DateTime.UtcNow, true);

            Mediator.GetInstance.Broadcast(Topic.ShellStateUpdated, heartbeat);
        }

        private readonly Action<Lazy<IDynamicViewModel>> _createView = ((t) =>
        {
            var vm = t.Value;

            Mediator.GetInstance.Register(vm);
            Window view = (Window)((BaseViewModel)vm).ViewReference;
            RunningWindows.TryAdd(vm.DynamicViewName, view);
            var heartbeat = new Heartbeat(vm.GetType().ToString(), String.Format("{0} View loaded at: {1}", vm.GetType().ToString(), DateTime.UtcNow.ToLongTimeString()), DateTime.UtcNow, true);

            Mediator.GetInstance.Broadcast(Topic.ShellStateUpdated, heartbeat);
            //view.Show();
            view.Closed += (sender, e) => view.Dispatcher.InvokeShutdown();
            Dispatcher.Run();
        });

        private void InjectDynamicViewModels(bool multiDispatchers)
        {
            foreach (var lazy in DynamicViewModels)
            {
                Lazy<IDynamicViewModel> localLazy = lazy;
                if (multiDispatchers)
                {
                    Mediator.GetInstance.Broadcast(Topic.BootstrapperLoadViews, localLazy); 
                }
                else
                {
                    CreateView(localLazy);
                }
                
            }
        }

        private void InjectServices()
        {
            foreach (var lazy in Services)
            {
                var service = lazy.Value;
                Mediator.GetInstance.Register(service);
                var heartbeat = new Heartbeat(service.GetType().ToString(), String.Format("{0} Service loaded at: {1}", service.GetType(), DateTime.UtcNow.ToLongTimeString()), DateTime.UtcNow, true);

                Mediator.GetInstance.Broadcast(Topic.ShellStateUpdated, heartbeat);
            }
            //inject service observers
            foreach (var lazy in ServiceObservers)
            {
                lazy.Value.AddServicesToObserve(Services.Select(s => s.Value));
            }
        }

        private void InjectStaticViewModels(MainWindow mainWindow)
        {
            foreach (var vm in StaticViewModels)
            {
                Mediator.GetInstance.Register(vm);
                mainWindow.RibbonRegion.Items.Add(new TabItem { Content = ((BaseViewModel)vm).ViewReference, Header = vm.StaticViewName });
            }
        }
    }
}
