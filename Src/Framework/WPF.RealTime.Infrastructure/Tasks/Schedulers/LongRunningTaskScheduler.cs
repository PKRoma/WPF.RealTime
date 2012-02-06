using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace WPF.RealTime.Infrastructure.Tasks.Schedulers
{
    public class LongRunningTaskScheduler : TaskScheduler
    {
        private readonly ApartmentState _apartmentState;
        private readonly ConcurrentQueue<Thread> _threads;
        private readonly ConcurrentQueue<Task> _tasks;

        public LongRunningTaskScheduler(ApartmentState apartmentState)
        {
            _apartmentState = apartmentState;
            _threads = new ConcurrentQueue<Thread>();
            _tasks = new ConcurrentQueue<Task>();
        }

        protected override void QueueTask(Task task)
        {
            Thread thread = new Thread(() => TryExecuteTask(task));
            thread.SetApartmentState(_apartmentState);
            thread.Name = "Module GUI Thread";
            thread.Start();
            _threads.Enqueue(thread);
            _tasks.Enqueue(task);
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return _tasks;
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            Thread thread = new Thread(() => TryExecuteTask(task));
            thread.SetApartmentState(_apartmentState);
            thread.Name = "Module GUI Thread";
            thread.Start();
            _threads.Enqueue(thread);
            _tasks.Enqueue(task);

            return true;
        }

    }
}
