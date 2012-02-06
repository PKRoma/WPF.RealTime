using System.ComponentModel;

namespace WPF.RealTime.Data.Entities
{
    public sealed class EntityCustomTypeDescriptor : CustomTypeDescriptor
    {
        private readonly PropertyDescriptorCollection _descriptors;

        public EntityCustomTypeDescriptor(PropertyDescriptorCollection descriptors)
        {
            _descriptors = descriptors;
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            return _descriptors;
        }
    }
}
