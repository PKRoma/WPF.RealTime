using System;

namespace WPF.RealTime.Data.Entities
{
    public class EntityKey
    {
        private readonly ulong _hashedValue;
        public ulong HashedValue
        {
            get { return _hashedValue; }
        }
        private readonly string _value;
        public string Value
        {
            get { return _value; }
        }
        private readonly KeyType _type;
        public KeyType KeyType
        {
            get { return _type; }
        }

        public EntityKey(string value, KeyType type)
        {
            if (value == null) throw new ArgumentNullException("key");
            _value = value;
            _hashedValue = EntityBuilder.Fnv1Hash(_value);
            _type = type;
        }
    }
}
