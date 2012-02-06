using System;
using System.Windows.Media;

namespace WPF.RealTime.Data.Binding
{
    [Serializable]
    public sealed class EntityItem
    {
        public object Value { get; private set; }
        public object OldValue { get; private set; }
        public Type Type { get; private set; }
        public String DisplayValue { get; set; }
        public Brush Background { get; set; }
        public Brush Foreground { get; set; }

        private void DefaultDisplay(object value)
        {
            if (value is string) DisplayValue = value.ToString();
            if (value is DateTime) DisplayValue = ((DateTime)value).ToString("d");
            if (value is double) DisplayValue = String.Format("{0:0.##}", value);
            if (value is Int32) DisplayValue = value.ToString();
        }

        public EntityItem(object value)
        {
            Value = value;
            Type = value.GetType();
            Background = Brushes.Transparent;
            Foreground = Brushes.Black;
            DefaultDisplay(value);
        }

        public EntityItem(object value, object oldValue)
        {
            Value = value;
            OldValue = oldValue;
            Type = value.GetType();
            Background = Brushes.Transparent;
            Foreground = Brushes.Black;
            DefaultDisplay(value);
        }
    }
}
