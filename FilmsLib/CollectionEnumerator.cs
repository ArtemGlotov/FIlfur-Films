using System;
using System.Collections;
using System.Collections.Generic;

namespace FilmsLib
{
    public class CollectionEnumerator<T> : IEnumerator<T> where T: IEquatable<T>
    {
        private MyCollection<T> _collection;
        private int _currentIndex;

        public CollectionEnumerator(MyCollection<T> collection)
        {
            _collection = collection;
            _currentIndex = -1;
        }

        public bool MoveNext() => ++_currentIndex < _collection.Count;

        public void Reset() => _currentIndex = -1;

        public T Current
        {
            get { return _collection[_currentIndex]; }
        }


        object IEnumerator.Current
        {
            get { return Current; }
        }

        void IDisposable.Dispose() { }
    }
}
