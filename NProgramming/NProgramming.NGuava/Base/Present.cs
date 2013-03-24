using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NProgramming.NGuava.Utils;

namespace NProgramming.NGuava.Base
{
    internal sealed class Present<T> : Optional<T> where T : class
    {
        private readonly T _reference;

        public Present(T reference)
        {
            _reference = reference;
        }

        public override bool IsPresent()
        {
            return true;
        }

        public override T Get()
        {
            return _reference;
        }

        public override T Or(T defaultValue)
        {
            Preconditions.CheckNotNull(defaultValue, "use Optional.OrNull() instead of Optional.Or(null)");
            return _reference;
        }

        public override Optional<T> Or(Optional<T> secondChoice)
        {
            Preconditions.CheckNotNull(secondChoice);
            return this;
        }

        public override T Or(ISupplier<T> supplier)
        {
            Preconditions.CheckNotNull(supplier);
            return _reference;
        }

        public override T OrNull()
        {
            return _reference;
        }

        public override ISet<T> AsSet()
        {
            var hashSet = new HashSet<T> {_reference};
            return new ReadOnlySet<T>(hashSet);
        }

        public override Optional<TResult> Transform<TResult>(Func<T, TResult> function)
        {
            return new Present<TResult>(Preconditions.CheckNotNull(function(_reference),
                                                                   "the Function passed to Optional<T>.Transform() must not return null."));
        }

        public override string ToString()
        {
            return "Optional.Of(" + _reference + ")";
        }

        public override bool Equals([Nullable] object @object)
        {
            if (@object is Present<T>) {
                var other = (Present<T>) @object;
                return _reference.Equals(other._reference);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return 0x598df91c + _reference.GetHashCode();
        }
    }
}
