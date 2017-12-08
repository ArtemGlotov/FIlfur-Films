using System;

namespace FilmsLib
{
    public class MyBase<T, U> : IEquatable<MyBase<T, U>>
    {
        public T first { get; }
        public U second { get; }
        public MyBase(T nFirst, U nSecond)
        {
            first = nFirst;
            second = nSecond;
        }

        public bool Equals(MyBase<T, U> other)
        {
            return first.Equals(other.first) && second.Equals(other.second);
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
