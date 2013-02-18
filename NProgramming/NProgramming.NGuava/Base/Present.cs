using System;
using System.Collections.Generic;

namespace NProgramming.NGuava.Base
{
    public sealed class Present<T> : Optional<T> where T : class
    {
        private readonly T reference;

        public Present(T reference)
        {
            this.reference = reference;
        }

        public override bool IsPresent()
        {
            return true;
        }

        public override T Get()
        {
            return reference;
        }

        public override T Or(T defaultValue)
        {
            Preconditions.CheckNotNull(defaultValue, "use Optional.OrNull() instead of Optional.Or(null)");
            return reference;
        }

        public override Optional<T> Or(Optional<T> secondChoice)
        {
            Preconditions.CheckNotNull(secondChoice);
            return this;
        }

        public override T Or(ISupplier<T> supplier)
        {
            Preconditions.CheckNotNull(supplier);
            return reference;
        }

        public override T OrNull()
        {
            return reference;
        }

        public override HashSet<T> AsSet()
        {
            return new HashSet<T> {reference};
        }

        public override Optional<TResult> Transform<TResult>(Func<T, TResult> function)
        {
            return new Present<TResult>(Preconditions.CheckNotNull(function(reference),
                                                                   "the Function passed to Optional.Transform() must not return null."));
        }

        public override string ToString()
        {
            return "Optional.Of(" + reference + ")";
        }

        public override bool Equals([Nullable] object @object)
        {
            if (@object is Present<T>) {
                var other = (Present<T>) @object;
                return reference.Equals(other.reference);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return 0x598df91c + reference.GetHashCode();
        }
    }
}
