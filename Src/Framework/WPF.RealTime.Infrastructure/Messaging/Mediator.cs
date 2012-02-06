using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WPF.RealTime.Infrastructure.Tasks;
using WPF.RealTime.Infrastructure.Tasks.Schedulers;

namespace WPF.RealTime.Infrastructure.Messaging
{
    public sealed class Mediator
    {
        
        #region Private Members Only
        private readonly Dictionary<string, List<WeakAction>> _registeredHandlers =
            new Dictionary<string, List<WeakAction>>();
        private readonly double _budget;
        private readonly Dictionary<object,Task<object>> _baseQueue =
            new Dictionary<object, Task<object>>();
        private readonly TaskFactory _backgroundTaskFactory;
        private readonly TaskFactory _staTaskFactory;
        private readonly PriorityTaskScheduler _preemptiveScheduler;
        #endregion

        #region .NET singleton
        private static readonly Mediator Instance = new Mediator();
        private Mediator()
        {
            _budget = Convert.ToDouble(ConfigurationManager.AppSettings["TASK_BUDGET"]);
            _backgroundTaskFactory =  new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);
            _staTaskFactory = new TaskFactory(new LongRunningTaskScheduler(ApartmentState.STA));
            _preemptiveScheduler = new PriorityTaskScheduler();
        }
        public static Mediator GetInstance
        {
            get {return Instance;}
        }
        #endregion

        public bool Broadcast(string key, params object[] message)
        {
            List<WeakAction> wr;
            lock (_registeredHandlers)
            {
                if (!_registeredHandlers.TryGetValue(key, out wr))
                    return false;
            }

            foreach (var cb in wr)
            {
                Delegate action = cb.GetMethod();
                if (action == null) continue;
                switch (cb.TaskType)
                {
                    case TaskType.Background:
                        // check if already running
                        if (!_baseQueue.ContainsKey(cb.Target.Target))
                        {
                            var bgTask = _backgroundTaskFactory.StartNew(() => action.DynamicInvoke(message));
                            _baseQueue.Add(cb.Target.Target, bgTask);
                        }
                        break;

                    case TaskType.Periodic:
                        var periodicTask = new PeriodicTask(() => action.DynamicInvoke(message), _budget);
                        periodicTask.Start(_preemptiveScheduler);
                        break;

                    case TaskType.Sporadic:
                        // one Periodic to run all sporadics
                        var sporadicTask = new SporadicTask(() => action.DynamicInvoke(message), _budget);
                        sporadicTask.Start(_preemptiveScheduler);
                        break;
                    case TaskType.LongRunning:
                        //UI
                        _staTaskFactory.StartNew(() => action.DynamicInvoke(message));
                        break;
                }

            }

            lock (_registeredHandlers)
            {
                wr.RemoveAll(wa => wa.HasBeenCollected);
            }

            return true;
        }

        public void RegisterInterest<T>(string key, Action<T> handler, TaskType taskType)
        {
            RegisterHandler(key, handler.GetType(), handler, taskType);
        }

        public void UnregisterInterest<T>(string key, Action<T> handler)
        {
            UnregisterHandler(key, handler.GetType(), handler);
        }

        public void Register(object obj)
        {
            // Look at all instance/static methods on this object type.
            foreach (var mi in obj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            {
                // See if we have a target attribute - if so, register the method as a handler.
                foreach (var att in mi.GetCustomAttributes(typeof(RegisterInterestAttribute), true))
                {
                    var ria = (RegisterInterestAttribute)att;
                    var pi = mi.GetParameters();
                    Type actionType = (pi.Length != 0) ?
                               GetType(pi.Length).MakeGenericType(pi.Select(p => p.ParameterType).ToArray()) : typeof(Action);

                    RegisterHandler(ria.Topic, actionType,
                                    mi.IsStatic
                                        ? Delegate.CreateDelegate(actionType, mi)
                                        : Delegate.CreateDelegate(actionType, obj, mi.Name),ria.TaskType);
                }
            }
        }

        public void Unregister(object obj)
        {
            foreach (var mi in obj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            {
                foreach (var att in mi.GetCustomAttributes(typeof(RegisterInterestAttribute), true))
                {
                    var ria = (RegisterInterestAttribute)att;
                    var pi = mi.GetParameters();
                    Type actionType = (pi.Length != 0) ?
                               GetType(pi.Length).MakeGenericType(pi.Select(p => p.ParameterType).ToArray()) : typeof(Action);

                    UnregisterHandler(ria.Topic, actionType,
                                      mi.IsStatic
                                          ? Delegate.CreateDelegate(actionType, mi)
                                          : Delegate.CreateDelegate(actionType, obj, mi.Name));
                }
            }
        }

        #region Private Methods
        private void RegisterHandler(string key, Type actionType, Delegate handler, TaskType taskType)
        {
            var action = new WeakAction(handler, actionType, taskType);

            lock (_registeredHandlers)
            {
                List<WeakAction> wr;
                if (_registeredHandlers.TryGetValue(key, out wr))
                {
                    if (wr.Count > 0)
                    {
                        WeakAction wa = wr[0];
                        if (wa.ActionType != actionType &&
                            !wa.ActionType.IsAssignableFrom(actionType))
                            throw new ArgumentException("Invalid key passed to RegisterHandler - existing handler has incompatible parameter type");
                    }

                    wr.Add(action);
                }
                else
                {
                    wr = new List<WeakAction> { action };
                    _registeredHandlers.Add(key, wr);
                }
            }
        }

        private void UnregisterHandler(string key, Type actionType, Delegate handler)
        {
            lock (_registeredHandlers)
            {
                List<WeakAction> wr;
                if (_registeredHandlers.TryGetValue(key, out wr))
                {
                    wr.RemoveAll(wa => handler == wa.GetMethod() && actionType == wa.ActionType);

                    if (wr.Count == 0)
                        _registeredHandlers.Remove(key);
                }
            }
        }

        private static Type GetType(int n)
        {
            switch (n)
            {
                case 1: return typeof(Action<>);
                case 2: return typeof(Action<,>);
                case 3: return typeof(Action<,,>);
                case 4: return typeof(Action<,,,>);
                case 5: return typeof(Action<,,,,>);
                case 6: return typeof(Action<,,,,,>);
                case 7: return typeof(Action<,,,,,,>);
                case 8: return typeof(Action<,,,,,,,>);
                case 9: return typeof(Action<,,,,,,,,>);
                case 10: return typeof(Action<,,,,,,,,,>);
                case 11: return typeof(Action<,,,,,,,,,,>);
                case 12: return typeof(Action<,,,,,,,,,,,>);
                case 13: return typeof(Action<,,,,,,,,,,,,>);
                case 14: return typeof(Action<,,,,,,,,,,,,,>);
                case 15: return typeof(Action<,,,,,,,,,,,,,,>);
                case 16: return typeof(Action<,,,,,,,,,,,,,,,>);
                default: throw new ApplicationException("Opps");
            }
        }
        #endregion
    }
}
