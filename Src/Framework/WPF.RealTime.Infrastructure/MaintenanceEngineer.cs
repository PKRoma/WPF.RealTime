using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using log4net;
using WPF.RealTime.Data;
using WPF.RealTime.Data.ResourceManager;
using WPF.RealTime.Infrastructure.Interfaces;
using WPF.RealTime.Infrastructure.Messaging;
using WPF.RealTime.Infrastructure.Utils;

namespace WPF.RealTime.Infrastructure
{
    public sealed class MaintenanceEngineer : IDispatcherFacade
    {
        private readonly ILog _log;
        private readonly Dispatcher _dispatcher;

        private readonly int _hr = Convert.ToInt32(ConfigurationManager.AppSettings["HEARBEAT_MONITOR_RATE"]);
        private readonly int _mr = Convert.ToInt32(ConfigurationManager.AppSettings["DISPATCHER_QUEUE_MONITOR_RATE"]);
        private readonly int _qs = Convert.ToInt32(ConfigurationManager.AppSettings["DISPATCHER_QUEUE_SIZE"]);
        private readonly string _dynamicViewName;

        public MaintenanceEngineer(string dynamicViewName, ILog log, Dispatcher dispatcher)
        {
            _dynamicViewName = dynamicViewName;
            _log = log;
            _dispatcher = dispatcher;

            Observable.Interval(TimeSpan.FromMilliseconds(_mr)).Subscribe(MonitorDispatcherQueue);
            Observable.Interval(TimeSpan.FromMilliseconds(_hr)).Subscribe(Heartbeat);
        }

        private long _operationsQueueCount = 0;
        private readonly ConcurrentDictionary<DispatcherOperation, object> _operations = new ConcurrentDictionary<DispatcherOperation, object>();

        private void Heartbeat(long l)
        {
            var heartbeat = new Heartbeat(_dynamicViewName, String.Format("{0} View heartbeat sent at: {1}", _dynamicViewName, DateTime.UtcNow.ToLongTimeString()), DateTime.UtcNow, false);
            Action w = () => Mediator.GetInstance.Broadcast(Topic.ShellStateUpdated, heartbeat);
            AddToDispatcherQueue(w);
        }

        private void MonitorDispatcherQueue(long l)
        {
            if (_operationsQueueCount != 0)
                _log.Info(String.Format("Dispatcher Operations In Queue {0}, ", _operationsQueueCount));

            if (_operationsQueueCount > _qs)
            {
                _log.Info("Pushing all Dispatcher operations");
                Application.Current.DoEvents();
                _operations.Clear();
                Interlocked.Exchange(ref _operationsQueueCount, 0);
            }
        }


        #region IDispatcherFacade Members

        public void AddToDispatcherQueue(Delegate workItem)
        {
            var operation = _dispatcher.BeginInvoke(DispatcherPriority.Background, workItem);

            operation.Completed += (s, o) =>
            {
                Interlocked.Decrement(ref _operationsQueueCount);
                object t;
                _operations.TryRemove((DispatcherOperation)s, out t);
            };
            _operations.TryAdd(operation, null);
            Interlocked.Increment(ref _operationsQueueCount);

        }
        #endregion
    }
}
