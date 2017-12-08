using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace FilmsLib
{
    public class MyCollection<T> : ICollection<T>, INotifyCollectionChanged where T: IEquatable<T>
    {
        private List<T> _elements;

        public MyCollection()
        {
            _elements = new List<T>();
        }

        public T this[int index]
        {
            get
            {
                return _elements[index];
            }
            set
            {
                T original = _elements[index];
                _elements[index] = value;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, _elements[index], original, index));
            }
        }

        public bool Contains(T consideredElement)
        {
            foreach (T thisElement in _elements)
            {
                if (thisElement.Equals(consideredElement))
                {
                    return true;
                }
            }
            return false;
        }

        public void Add(T newElement)
        {
            if (!Contains(newElement))
            {
                _elements.Add(newElement);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newElement, Count - 1));
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            foreach(T item in collection)
            {
                Add(item);
            }
        }

        public void Clear()
        {
            _elements.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public int IndexOf(T consideredElement)
        {
            int index = 0;
            foreach(T element in this)
            {
                if(!consideredElement.Equals(element))
                {
                    index++;
                }
                else
                {
                    return index;
                }
            }
            return -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("The array cannot be null.");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("The starting array index cannot be negative.");
            if (Count > array.Length - arrayIndex + 1)
                throw new ArgumentException("The destination array has fewer elements than the collection.");

            for (int i = 0; i < _elements.Count; i++)
            {
                array[i + arrayIndex] = _elements[i];
            }
        }

        public int Count
        {
            get
            {
                return _elements.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Remove(T removedElement)
        {
            if(Contains(removedElement))
            { 
                for (int i = 0; i < _elements.Count; i++)
                {
                    if (_elements[i].Equals(removedElement))
                    {
                        T removedItem = _elements[i];
                        _elements.RemoveAt(i);
                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedElement, i));
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Remove(int index)
        {
            if (index > -1 && index < _elements.Count)
            {
                _elements.RemoveAt(index);
                return true;
            }
            else return false;
        }

        public IEnumerator GetEnumerator() => new CollectionEnumerator<T>(this);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => new CollectionEnumerator<T>(this);

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
                CollectionChanged?.Invoke(this, args);
        }

        public void Sort(IComparer<T> nComparer)
        {
            T temp;
            MyCollection<T> oldCollection = new MyCollection<T>();
            foreach(T item in _elements)
            {
                oldCollection.Add(item);
            }
            for(int i = 0; i < Count; i++)
            {
                for(int j = i + 1; j < Count; j++)
                {
                    if(nComparer.Compare(_elements[i],_elements[j]) == 1)
                    {
                        temp = this[i];
                        this[i] = this[j];
                        this[j] = temp;
                    }
                }
            }
        }

    }
        
    public static class IEnumerableExtension
    {
        public static MyCollection<T> AsMyCollection<T>(this IEnumerable<T> collection) where T : IEquatable<T>
        {
            MyCollection<T> resultColl = new MyCollection<T>();
            resultColl.AddRange(collection);
            return resultColl;
        }
    }

}

