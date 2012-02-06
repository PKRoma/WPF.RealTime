using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WPF.RealTime.Data.Binding;
using WPF.RealTime.Data.Interfaces;

namespace WPF.RealTime.Data.Entities
{
    [Serializable]
    public sealed class Entity : SelfExplained
    {
        private readonly EntityKey _key;
        private readonly ConcurrentDictionary<string, EntityItem> _values = new ConcurrentDictionary<string, EntityItem>();

        public ConcurrentDictionary<string, EntityItem> Values
        {
            get { return _values; }
        }

        public EntityKey Key
        {
            get { return _key; }
        }

        public Entity(EntityKey key)
        {
            if (key == null) throw new ArgumentNullException("key");
            _key = key;
            _values.TryAdd(key.KeyType.ToString(), new EntityItem(key.Value));
        }

        #region ISelfExplained Members
        public override IEnumerable<string> GetPropertyNames()
        {
            return _values.Keys;
        }
        public override IEnumerable<object> GetPropertyValues()
        {
            return _values.Values;
        }
        #endregion
    }
}
