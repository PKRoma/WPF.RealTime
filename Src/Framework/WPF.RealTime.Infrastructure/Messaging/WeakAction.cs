using System;

namespace WPF.RealTime.Infrastructure.Messaging
{
    public sealed class WeakAction
    {
        #region Private Members Only
        private readonly WeakReference _target;
        private readonly Type _ownerType;
        private readonly Type _actionType;
        private readonly string _methodName;
        private readonly TaskType _taskType;
        #endregion

        #region Public Properties/Methods
        public WeakAction(Delegate handler, Type actionType, TaskType taskType)
        {
            if (handler.Method.IsStatic)
                _ownerType = handler.Method.DeclaringType;
            else
                _target = new WeakReference(handler.Target);

            _methodName = handler.Method.Name;
            _actionType = actionType;
            _taskType = taskType;
        }

        public WeakReference Target
        {
            get { return _target; }
        }

        public TaskType TaskType
        {
            get { return _taskType; }
        }

        public Type ActionType
        {
            get { return _actionType; }
        }

        public bool HasBeenCollected
        {
            get
            {
                return (_ownerType == null && (_target == null || !_target.IsAlive));
            }
        }

        public Delegate GetMethod()
        {
            if (_ownerType != null)
            {
                return Delegate.CreateDelegate(_actionType, _ownerType, _methodName);
            }

            if (_target != null && _target.IsAlive)
            {
                object target = _target.Target;
                if (target != null)
                    return Delegate.CreateDelegate(_actionType, target, _methodName);
            }

            return null;
        }
        #endregion
    }
}
