using System;
using System.Threading.Tasks;

namespace WPF.RealTime.Infrastructure.Tasks
{
    public class PeriodicTask : Task
    {
        private readonly long _timeToRun;
        public long TimeToRun { get { return _timeToRun; } }
        private readonly DateTime _deadline;
        public DateTime Deadline { get { return _deadline; } }
        private readonly double _budget;
        public double Budget { get { return _budget; } }
        private readonly Action _action;
        public Action Action { get { return _action; } }
        private readonly int _priority;
        public int Priority { get { return _priority; } }
        private readonly DateTime _timeCreated;
        public DateTime TimeCreated { get { return _timeCreated; } }

        public PeriodicTask(Action action, double budget) : base(action)
        {
            _timeToRun = 0;
            _deadline = DateTime.UtcNow.AddMilliseconds(budget);
            _budget = budget;
            _priority = 0;
            _action = action;
            _timeCreated = DateTime.UtcNow;
        }
    }
}
