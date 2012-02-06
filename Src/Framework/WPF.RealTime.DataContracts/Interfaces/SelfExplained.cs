using System.Collections.Generic;
using System.ComponentModel;

namespace WPF.RealTime.Data.Interfaces
{
    public abstract class SelfExplained : INotifyPropertyChanged
    {
        public abstract IEnumerable<string> GetPropertyNames();
        public abstract IEnumerable<object> GetPropertyValues();

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
