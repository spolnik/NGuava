using System;
using System.Collections.Generic;
using NProgramming.NGuava.Utils;

namespace NProgramming.NGuava.Base
{
    internal sealed class Absent<T> : Optional<T> where T : class
    {
        internal static Absent<T> Instance = new Absent<T>();

        private Absent() {}

        public override bool IsPresent()
        {
            return false;
        }

        public override T Get()
        {
            throw new InvalidOperationException("Optional.Get() cannot be called on an absent value");
        }

        public override T Or(T defaultValue)
        {
            return Preconditions.CheckNotNull(defaultValue, "use Optional.OrNull() instead of Optional.Or(null)");
        }

        public override Optional<T> Or(Optional<T> secondChoice)
        {
            return Preconditions.CheckNotNull(secondChoice);
        }

        public override T Or(ISupplier<T> supplier)
        {
            return Preconditions.CheckNotNull(supplier.Get(),
                                              "use Optional.OrNull() instead of a Supplier that returns null");
        }

        public override T OrNull()
        {
            return null;
        }

        public override ISet<T> AsSet()
        {
            return new ReadOnlySet<T>();
        }

        public override Optional<TResult> Transform<TResult>(Func<T, TResult> function)
        {
            Preconditions.CheckNotNull(function);
            return Optional<TResult>.Absent();
        }

        public override string ToString()
        {
            return "Optional.Absent()";
        }

        public override bool Equals([Nullable] object @object)
        {
            return @object == this;
        }

        public override int GetHashCode()
        {
            return 0x598df91c;
        }
    }
}