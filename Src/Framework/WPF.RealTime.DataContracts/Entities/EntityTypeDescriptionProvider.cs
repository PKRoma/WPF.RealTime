using System;
using System.ComponentModel;

namespace WPF.RealTime.Data.Entities
{
    public class EntityTypeDescriptionProvider : TypeDescriptionProvider
    {
        private readonly ICustomTypeDescriptor _defaultDescriptor;

        public EntityTypeDescriptionProvider(TypeDescriptionProvider parent, ICustomTypeDescriptor defaultDescriptor)
            : base(parent)
        {
            _defaultDescriptor = defaultDescriptor;
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            return _defaultDescriptor;
        }
    }
}
