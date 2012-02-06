using System;
using System.ComponentModel;

namespace WPF.RealTime.Data.Binding
{
    public class DataItem : INotifyPropertyChanged, IDataErrorInfo
    {
        #region private members
        private object _value;
        private readonly Type _type;
        #endregion

        #region Constructors
        public DataItem(object value)
        {
            _value = value;
            _type = value.GetType();
        }
        #endregion

        #region IItem Members

        public object Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        public Type Type
        {
            get { return _type; }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IDataErrorInfo Members
        //WPF doen't use this property
        public string Error
        {
            get { return null; }
        }

        public string this[string propertyName]
        {
            get
            {
                if (propertyName == "Value")
                {
                    if (Value == null)
                        return "The Value property can not be null.";
                }
                return null;
            }
        }

        #endregion
    }
}
