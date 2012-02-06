using System;
using System.Threading.Tasks;

namespace WPF.RealTime.Infrastructure.Tasks
{

    public class SporadicTask : Task
    {
        private readonly double _budget;
        public double EstimatedBudget { get { return _budget; } }
        private readonly Action _action;
        public Action Action { get { return _action; } }
        private readonly int _priority;
        public int Priority { get { return _priority; } }
        private readonly DateTime _timeCreated;
        public DateTime TimeCreated { get { return _timeCreated; } }

        public SporadicTask(Action action, double budget)
            : base(action)
        {
            _budget = budget;
            _priority = 0;
            _action = action;
            _timeCreated = DateTime.UtcNow;
        }
    }
}
