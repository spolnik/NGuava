using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NProgramming.NGuava.Utils
{
    internal class ReadOnlySet<T> : ISet<T>
    {
        private readonly HashSet<T> _set;

        public ReadOnlySet()
        {
            _set = new HashSet<T>();
        }

        public ReadOnlySet(HashSet<T> set)
        {
            _set = set;
        }

        public IEqualityComparer<T> Comparer
        {
            get { return _set.Comparer; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _set).GetEnumerator();
        }

        public void Clear()
        {
            throw ReadOnlyException();
        }

        public bool Contains(T item)
        {
            return _set.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _set.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            throw ReadOnlyException();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _set.GetEnumerator();
        }

        bool ISet<T>.Add(T item)
        {
            throw ReadOnlyException();
        }

        public void UnionWith(IEnumerable<T> other)
        {
            throw ReadOnlyException();
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            throw ReadOnlyException();
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            throw ReadOnlyException();
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw ReadOnlyException();
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return _set.IsSubsetOf(other);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return _set.IsProperSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return _set.IsSupersetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return _set.IsProperSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return _set.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return _set.SetEquals(other);
        }

        public int Count
        {
            get { return _set.Count; }
        }

        public void Add(T item)
        {
            throw ReadOnlyException();
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            _set.GetObjectData(info, context);
        }

        public void OnDeserialization(object sender)
        {
            _set.OnDeserialization(sender);
        }

        public void CopyTo(T[] array)
        {
            _set.CopyTo(array);
        }

        public void CopyTo(T[] array, int arrayIndex, int count)
        {
            _set.CopyTo(array, arrayIndex, count);
        }

        public int RemoveWhere(Predicate<T> match)
        {
            throw ReadOnlyException();
        }

        public void TrimExcess()
        {
            _set.TrimExcess();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _set.GetEnumerator();
        }

        private static Exception ReadOnlyException()
        {
            return new NotSupportedException("This set is read-only");
        }
    }
}