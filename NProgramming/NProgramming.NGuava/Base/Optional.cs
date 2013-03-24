using System;
using System.Collections.Generic;
using System.Linq;

namespace NProgramming.NGuava.Base
{
    [Serializable]
    public abstract class Optional<T> where T : class
    {
        public static Optional<T> Absent()
        {
            return Absent<T>.Instance;
        }

        public static Optional<T> Of(T reference)
        {
            return new Present<T>(Preconditions.CheckNotNull(reference));
        }

        public static Optional<T> FromNullable([Nullable] T nullableReference)
        {
            return (nullableReference == null)
                       ? Absent()
                       : new Present<T>(nullableReference);
        }

        public abstract bool IsPresent();

        public abstract T Get();

        public abstract T Or(T defaultValue);

        public abstract Optional<T> Or(Optional<T> secondChoice);

        public abstract T Or(ISupplier<T> supplier);

        [return: Nullable]
        public abstract T OrNull();

        public abstract ISet<T> AsSet();

        public abstract Optional<TResult> Transform<TResult>(Func<T, TResult> function) where TResult : class;

        public abstract override bool Equals([Nullable] object @object);

        public abstract override int GetHashCode();

        public abstract override string ToString();

        public static IEnumerable<T> PresentInstances(IEnumerable<Optional<T>> optionals)
        {
            Preconditions.CheckNotNull(optionals);

            return optionals.Where(x => x.IsPresent()).Select(x => x.Get());
        }
    }
}