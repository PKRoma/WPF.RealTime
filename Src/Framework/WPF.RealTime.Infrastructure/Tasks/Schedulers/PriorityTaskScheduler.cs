using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using WPF.RealTime.Infrastructure.Collections;

namespace WPF.RealTime.Infrastructure.Tasks.Schedulers
{
    public sealed class PriorityTaskScheduler : TaskScheduler
    {
        #region Private Members Only
        private readonly int _buferSize;
        private readonly double _threshold;
        private readonly ConcurrentQueue<Task> _waitingQueue;
        private readonly BlockingCircularBuffer<PeriodicTask> _readyToRunQueue;
        private readonly ILog _log = LogManager.GetLogger(typeof(PriorityTaskScheduler));
        private readonly PerformanceCounter _messageCounter;
        private readonly TaskFactory _backgroundTaskFactory =
            new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);
        private readonly int _tsp;
        #endregion

        public PriorityTaskScheduler()
        {
            //TODO: create category
            //_messageCounter = new PerformanceCounter();
            _tsp = Convert.ToInt32(ConfigurationManager.AppSettings["TASK_SCHEDULER_PERIOD"]);
            _buferSize = Convert.ToInt32(ConfigurationManager.AppSettings["BUFFER_SIZE"]);
            _threshold = Convert.ToDouble(ConfigurationManager.AppSettings["SUSPENSION_THRESHOLD"]);
            _readyToRunQueue = new BlockingCircularBuffer<PeriodicTask>(_buferSize);
            _waitingQueue = new ConcurrentQueue<Task>();

            var executor = new System.Timers.Timer(_tsp);
            executor.Elapsed += WaitingQueueToReadyToRun;
            executor.Start();

            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                _backgroundTaskFactory.StartNew(() =>
                {
                    while (true)
                    {
                        var task = _readyToRunQueue.Dequeue();
#if DEBUG
                        _log.Debug("Message Dequeued");
                        //_messageCounter.Decrement();
#endif    
                        if (TryExecuteTask(task))
                        {
                            var span = DateTime.UtcNow - task.TimeCreated;
                            if (DateTime.UtcNow > task.Deadline)
                            {
                                _log.Warn(String.Format("Real-time Deadline exceeded : {0}", span.TotalMilliseconds));
                                //throw new ApplicationException("Real-time Deadline exceeded");
                            }
#if DEBUG
                            _log.Debug("Message Done");
#endif
                        }
                    }
                });
            }    
        }

        private void WaitingQueueToReadyToRun(object sender, System.Timers.ElapsedEventArgs e)
        {
            Task task;
            while (_waitingQueue.TryDequeue(out task))
            {
                // check budget and invoke a context switch
                PeriodicTask headTask;
                if (task is PeriodicTask)
                {
                    headTask = (PeriodicTask)task;
                    var nextToRun = _readyToRunQueue.Peek();

                    if ((nextToRun != null) && (nextToRun.Status == TaskStatus.WaitingToRun)
                        && headTask.Deadline < nextToRun.Deadline)
                    {
                        _log.Info("Context switching at: " + DateTime.UtcNow);
                        var dequeuedTask = _readyToRunQueue.Dequeue();
                        _readyToRunQueue.Enqueue(headTask);
                        _readyToRunQueue.Enqueue(dequeuedTask);
                    }
                    else
                    {
                        _readyToRunQueue.Enqueue(headTask);
                    }
                    
                }  
            }
        }

        protected override void QueueTask(Task task)
        {
#if DEBUG
            _log.Debug("Message Enqueued");
            //_messageCounter.Increment();
#endif
            // jump the queue
            if (task is SporadicTask)
            {
                TryExecuteTask(task);
                _log.Info("Sporadic jumped the queue at: " + DateTime.UtcNow);
                return;
            }
            
            _waitingQueue.Enqueue(task); 
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return TryExecuteTask(task);
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return _waitingQueue.ToArray();
        }
    }
}
