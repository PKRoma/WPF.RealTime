using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using WPF.RealTime.Data;
using WPF.RealTime.Data.Binding;
using WPF.RealTime.Data.ResourceManager;
using WPF.RealTime.Infrastructure;
using WPF.RealTime.Infrastructure.AttachedCommand;
using WPF.RealTime.Infrastructure.Commands;
using WPF.RealTime.Infrastructure.Messaging;

namespace WPF.RealTime.Shell
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly int _hr = Convert.ToInt32(ConfigurationManager.AppSettings["HEARBEAT_MONITOR_RATE"]);
        private readonly int _ht = Convert.ToInt32(ConfigurationManager.AppSettings["HEARBEAT_THRESHOLD"]);

        public ObservableCollection<SelectableDataItem> Themes { get; private set; }
        public ObservableCollection<SelectableDataItem> Heartbeats { get; private set; }
        public ICommand ChangeThemeCommand { get; private set; }
        public ICommand ReloadCommand { get; private set; }

        private string _staleModule;
        public string StaleModule
        {
            get { return _staleModule; }
            set { _staleModule = value; OnPropertyChanged("StaleModule"); }
        }
        private Visibility _heartbeatLost;
        public Visibility HeartbeatLost
        {
            get { return _heartbeatLost; }
            set { _heartbeatLost = value; OnPropertyChanged("HeartbeatLost"); }
        }

        private readonly ConcurrentDictionary<string, Heartbeat> _hearbeatIndex;
        public MainWindowViewModel() : base("Shell", false, true)
        {
            _hearbeatIndex = new ConcurrentDictionary<string, Heartbeat>();

            Mediator.GetInstance.RegisterInterest<Heartbeat>(Topic.ShellStateUpdated, HeartbeatReceived, TaskType.Periodic);

            HeartbeatLost = Visibility.Collapsed;
            Heartbeats = new ObservableCollection<SelectableDataItem>();

            Themes = new ObservableCollection<SelectableDataItem>
                         {
                             new SelectableDataItem(Constants.ThemeAero),
                             new SelectableDataItem(Constants.ThemeLuna),
                             new SelectableDataItem(Constants.ThemeRoyale),
                             //new SelectableDataItem(Constants.ThemeGold) // not ready
                         };

            ChangeThemeCommand = new SimpleCommand<Object, EventToCommandArgs>(ChangeTheme);
            ReloadCommand = new SimpleCommand<Object>(_ => 
                {
                    Heartbeat removed;
                    _hearbeatIndex.TryRemove(StaleModule, out removed);
                    HeartbeatLost = Visibility.Collapsed;
                    Mediator.GetInstance.Broadcast(Topic.BootstrapperUnloadView, StaleModule);
                });

            var timer = new Timer(_hr);
            timer.Elapsed += (s, e) =>
                                 {
                                     var lostHeartbeats = _hearbeatIndex.Values
                                         .Where(i => (!i.NonRepeatable) && (DateTime.UtcNow - i.TimeCreated).TotalMilliseconds > _ht);
                                     foreach (var l in lostHeartbeats)
                                     {
                                         HeartbeatLost = Visibility.Visible;
                                         StaleModule = l.Key;
                                         Log.Warn(String.Format("Lost heartbeat from: {0}",l.Key));
                                     }
                                 };
            timer.Start();
        }

        private void ChangeTheme(EventToCommandArgs _)
        {
            ProcessOnDispatcherThread(()=>
                {
                    var t = Themes.Where(s=>s.IsSelected).FirstOrDefault();
                    if (t != null)
                        ((App)Application.Current).ChangeTheme(t.Value.ToString());
                });
        }

        public void HeartbeatReceived(Heartbeat heartbeat)
        {
            Action w = () =>
                {
                    if (!_hearbeatIndex.ContainsKey(heartbeat.Key))
                    {
                        Heartbeats.Add(new SelectableDataItem(heartbeat.Message));
                        _hearbeatIndex.TryAdd(heartbeat.Key, heartbeat);
                    }
                    else
                    {
                        var item = Heartbeats.Where(s => s.Value.ToString().Contains(heartbeat.Key)).FirstOrDefault();
                        item.Value = heartbeat.Message;
                        _hearbeatIndex.AddOrUpdate(heartbeat.Key, heartbeat, (n, oldValue) => heartbeat);

                        // resuscitate
                        if (heartbeat.Key == StaleModule) HeartbeatLost = Visibility.Collapsed;
                    }
                };
            DispatcherFacade.AddToDispatcherQueue(w);
        }
    }
}
