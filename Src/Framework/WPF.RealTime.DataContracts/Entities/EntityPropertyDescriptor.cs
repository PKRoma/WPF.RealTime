using System;
using System.ComponentModel;
using WPF.RealTime.Data.Binding;

namespace WPF.RealTime.Data.Entities
{
    public class EntityPropertyDescriptor : PropertyDescriptor
    {
        private readonly Type _type;
        private readonly bool _readOnly;

        public EntityPropertyDescriptor(string name, bool readOnly, Type type, Attribute[] attributes): base(name, attributes)
        {
            _type = type;
            _readOnly = readOnly;
        }

        public override object GetValue(object component)
        {
            var entity = (Entity)component;
            EntityItem value;
            entity.Values.TryGetValue(Name, out value);
            return value;
        }

        public override void SetValue(object component, object value)
        {
            var entity = (Entity)component;
            entity.Values[Name] = (EntityItem)value;
        }

        public override void ResetValue(object component)
        {
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override Type PropertyType
        {
            get { return _type; }
        }

        public override bool IsReadOnly
        {
            get { return _readOnly; }
        }

        public override Type ComponentType
        {
            get { return typeof(Entity); }
        }
    }
}
