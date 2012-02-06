using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using WPF.RealTime.Data.Interfaces;

namespace WPF.RealTime.Infrastructure.Collections
{
    public class NotifyCollection<T> : Collection<T>, ITypedList, INotifyCollectionChanged where T : SelfExplained
    {
        private readonly PropertyDescriptorCollection _descriptors;

        public NotifyCollection(PropertyDescriptorCollection descriptors)
        {
            _descriptors = descriptors;
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
            
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items));
        }

        public void AddOrUpdate(IEnumerable<T> items, bool isReplace)
        {
            foreach (var item in items)
            {
                if (!Contains(item))
                {
                    Add(item);
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
                }
                else
                {
                    if (isReplace)
                    {
                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, item));
                    }
                    else
                    {
                        foreach (var property in item.GetPropertyNames())
                        {
                            item.OnPropertyChanged(property);
                        }
                    }
                }
            }
        }

        public void AddOrUpdate(T item, bool isReplace)
        {
            if (!Contains(item))
            {
                Add(item);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            }
            else
            {
                if (isReplace)
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, item));  
                }
                else
                {
                    foreach (var property in item.GetPropertyNames())
                    {
                        item.OnPropertyChanged(property);
                    }
                }
            }
        }

        #region INotifyCollectionChanged Members
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, e);
            }
        }
        #endregion

        #region ITypedList Members

        string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
        {
            return typeof(T).Name;
        }

        PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            return _descriptors;
        }

        #endregion
    }
}
