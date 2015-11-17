using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Gilgame.SEWorkbench.Services
{
    /// credit to romkyns over at stack overflow
    /// https://bitbucket.org/rstarkov/wpfcrutches/src/tip/ObservableSortedList.cs?fileviewer=file-view-default
    /// 
    /// <summary>
    ///     Implements an observable collection which maintains its items in sorted order. In particular, items remain sorted
    ///     when changes are made to their properties: they are reordered automatically when necessary to keep them sorted.</summary>
    /// <remarks>
    ///     <para>This class currently requires <typeparamref name="T" /> to be a reference type. This is because a couple of
    ///     methods operate on the basis of reference equality instead of the comparison used for sorting. As implemented,
    ///     their behaviour for value types would be somewhat unexpected.</para>
    ///     <para>The INotifyCollectionChange interface is fairly complicated and relatively poorly documented (see
    ///     http://stackoverflow.com/a/5883947/33080 for example), increasing the likelihood of bugs. And there are currently
    ///     no unit tests. There could well be bugs in this code.</para></remarks>
    [Serializable]
    public class ObservableSortedList<T> : IList<T>, INotifyPropertyChanged, INotifyCollectionChanged where T : class, INotifyPropertyChanged
    {
        private List<T> _list;
        private IComparer<T> _comparer;

        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public ObservableSortedList()
            : this(4)
        {
            // pass it on
        }

        public ObservableSortedList(IComparer<T> comparer) : this(4, comparer)
        {
            // pass it on
        }

        public ObservableSortedList(int capacity = 4, IComparer<T> comparer = null)
        {
            _list = new List<T>(capacity);
            _comparer = comparer ?? Comparer<T>.Default;
        }

        public ObservableSortedList(IEnumerable<T> items, IComparer<T> comparer = null)
        {
            _list = new List<T>(items);
            _comparer = comparer ?? Comparer<T>.Default;
            _list.Sort(_comparer);

            foreach (var item in _list)
            {
                item.PropertyChanged += ItemPropertyChanged;
            }
        }

        public void Clear()
        {
            foreach (var item in _list)
            {
                item.PropertyChanged -= ItemPropertyChanged;
            }
            _list.Clear();

            collectionChanged_Reset();
            propertyChanged("Count");
        }

        public void Add(T item)
        {
            int i = _list.BinarySearch(item, _comparer);
            if (i < 0)
            {
                i = ~i;
            }
            else
            {
                do i++; while (i < _list.Count && _comparer.Compare(_list[i], item) == 0);
            }
            _list.Insert(i, item);

            item.PropertyChanged += ItemPropertyChanged;

            collectionChanged_Added(item, i);
            propertyChanged("Count");
        }

        public void Insert(int index, T item)
        {
            throw new InvalidOperationException("Cannot insert an item at an arbitrary index into a ObservableSortedList.");
        }

        public bool Remove(T item)
        {
            int i = IndexOf(item);
            if (i < 0)
            {
                return false;
            }

            item.PropertyChanged -= ItemPropertyChanged;

            _list.RemoveAt(i);

            collectionChanged_Removed(item, i);
            propertyChanged("Count");

            return true;
        }

        public void RemoveAt(int index)
        {
            var item = _list[index];

            item.PropertyChanged -= ItemPropertyChanged;

            _list.RemoveAt(index);

            collectionChanged_Removed(item, index);
            propertyChanged("Count");
        }

        public T this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                throw new InvalidOperationException("Cannot set an item at an arbitrary index in a ObservableSortedList.");
            }
        }

        public int IndexOf(T item)
        {
            int i = _list.BinarySearch(item, _comparer);
            if (i < 0)
            {
                return -1;
            }

            if (object.ReferenceEquals(_list[i], item))
            {
                return i;
            }

            // Search downwards
            for (int s = i - 1; s >= 0 && _comparer.Compare(_list[s], item) == 0; s--)
            {
                if (object.ReferenceEquals(_list[s], item))
                {
                    return s;
                }
            }

            // Search upwards
            for (int s = i + 1; s < _list.Count && _comparer.Compare(_list[s], item) == 0; s++)
            {
                if (object.ReferenceEquals(_list[s], item))
                {
                    return s;
                }
            }

            // Not found
            return -1;
        }

        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void propertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void collectionChanged_Reset()
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        private void collectionChanged_Added(T item, int index)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            }
        }

        private void collectionChanged_Removed(T item, int index)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
            }
        }

        private void collectionChanged_Moved(T item, int oldIndex, int newIndex)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, newIndex, oldIndex));
            }
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = (T)sender;
            int oldIndex = _list.IndexOf(item);

            // See if item should now be sorted to a different position
            if (Count <= 1 || (oldIndex == 0 || _comparer.Compare(_list[oldIndex - 1], item) <= 0) && (oldIndex == Count - 1 || _comparer.Compare(item, _list[oldIndex + 1]) <= 0))
            {
                return;
            }

            // Find where it should be inserted 
            _list.RemoveAt(oldIndex);
            int newIndex = _list.BinarySearch(item, _comparer);
            if (newIndex < 0)
            {
                newIndex = ~newIndex;
            }
            else
            {
                do newIndex++; while (newIndex < _list.Count && _comparer.Compare(_list[newIndex], item) == 0);
            }

            _list.Insert(newIndex, item);
            collectionChanged_Moved(item, oldIndex, newIndex);
        }
    }
}
