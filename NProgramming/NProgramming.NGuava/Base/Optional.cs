using System;
using System.Collections.Generic;
using System.Linq;

namespace NProgramming.NGuava.Base
{
    [Serializable]
    public abstract class Optional<TClass>
    {
        public static Optional<T> Absent<T>() where T : class
        {
            return Base.Absent<T>.Instance;
        }

        public static Optional<T> Of<T>(T reference) where T : class
        {
            return new Present<T>(Preconditions.CheckNotNull(reference));
        }

        public static Optional<T> FromNullable<T>([Nullable] T nullableReference) where T : class
        {
            return (nullableReference == null)
                       ? Optional<T>.Absent<T>()
                       : new Present<T>(nullableReference);
        }

        public abstract bool IsPresent();

        public abstract TClass Get();

        public abstract TClass Or(TClass defaultValue);

        public abstract Optional<TClass> Or(Optional<TClass> secondChoice);

        public abstract TClass Or(ISupplier<TClass> supplier);

        [return: Nullable]
        public abstract TClass OrNull();

        public abstract HashSet<TClass> AsSet();

        public abstract Optional<TResult> Transform<TResult>(Func<TClass, TResult> function) where TResult : class;

        public abstract override bool Equals([Nullable] object @object);

        public abstract override int GetHashCode();

        public abstract override string ToString();

        public static IEnumerable<T> PresentInstances<T>(IEnumerable<Optional<T>> optionals)
        {
            Preconditions.CheckNotNull(optionals);

            return optionals.Where(x => x.IsPresent()).Select(x => x.Get());
        }
    }
}