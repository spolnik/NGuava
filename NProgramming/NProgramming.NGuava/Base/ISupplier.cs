using System;
using NProgramming.NGuava.Utils;

namespace NProgramming.NGuava.Base
{
    public interface ISupplier<out T>
    {
        T Get();
    }

    public static class Suppliers
    {
        public static ISupplier<T> Compose<T1, T>(Func<T1, T> function, ISupplier<T1> supplier)
        {
            Preconditions.CheckNotNull(function);
            Preconditions.CheckNotNull(supplier);

            return new SupplierComposition<T1, T>(function, supplier);
        }

        private class SupplierComposition<T1, T> : ISupplier<T>
        {
            private readonly Func<T1, T> _function;
            private readonly ISupplier<T1> _supplier;

            public SupplierComposition(Func<T1, T> function, ISupplier<T1> supplier)
            {
                _function = function;
                _supplier = supplier;
            }

            public T Get()
            {
                return _function(_supplier.Get());
            }

            public override bool Equals(object obj)
            {
                if (obj is SupplierComposition<T1, T>)
                {
                    var that = (SupplierComposition<T1, T>)obj;
                    return _function.Equals(that._function) &&
                           _supplier.Equals(that._supplier);
                }

                return false;
            }

            public override int GetHashCode()
            {
                return Objects.HashCode(_function, _supplier);
            }

            public override string ToString()
            {
                return "Suppliers.Compose(" + _function + ", " + _supplier + ")";
            }
        }

        public static ISupplier<T> Memoize<T>(ISupplier<T> @delegate)
        {
            return (@delegate is MemoizingSupplier<T>)
                       ? @delegate
                       : new MemoizingSupplier<T>(Preconditions.CheckNotNull(@delegate));
        }

        private class MemoizingSupplier<T> : ISupplier<T>
        {
            private readonly ISupplier<T> _delegate;
            private volatile bool _initialized;
            private T _value;

            public MemoizingSupplier(ISupplier<T> @delegate)
            {
                _delegate = @delegate;
            }


            public T Get()
            {
                if (!_initialized)
                {
                    lock (this)
                    {
                        if (!_initialized)
                        {
                            var t = _delegate.Get();
                            _value = t;
                            _initialized = true;
                            return t;
                        }
                    }
                }

                return _value;
            }

            public override string ToString()
            {
                return "Suppliers.Memoize(" + _delegate + ")";
            }
        }

        public static ISupplier<T> MemoizeWithExpiration<T>(ISupplier<T> @delegate, TimeSpan duration)
        {
            return new ExpiringMemoizingSupplier<T>(@delegate, duration);
        }

        private class ExpiringMemoizingSupplier<T> : ISupplier<T>
        {
            private readonly ISupplier<T> _delegate;
            private readonly long _durationTicks;
            private T _value;
            private long _expirationTicks;

            public ExpiringMemoizingSupplier(ISupplier<T> @delegate, TimeSpan duration)
            {
                _delegate = Preconditions.CheckNotNull(@delegate);
                _durationTicks = duration.Ticks;
                Preconditions.CheckArgument(duration.TotalMilliseconds > 0);
            }

            public T Get()
            {
                // Another variant of Double Checked Locking.
                //
                // We use two volatile reads.  We could reduce this to one by
                // putting our fields into a holder class, but (at least on x86)
                // the extra memory consumption and indirection are more
                // expensive than the extra volatile reads.
                long ticks = _expirationTicks;
                long now = DateTime.Now.Ticks;
                if (ticks == 0 || now - ticks >= 0)
                {
                    lock (this)
                    {
                        if (ticks == _expirationTicks)
                        {
                            // recheck for lost race
                            T t = _delegate.Get();
                            _value = t;
                            ticks = now + _durationTicks;
                            // In the very unlikely event that nanos is 0, set it to 1;
                            // no one will notice 1 ns of tardiness.
                            _expirationTicks = (ticks == 0) ? 1 : ticks;
                            return t;
                        }
                    }
                }

                return _value;
            }

            public override string ToString()
            {
                return "Suppliers.MemoizeWithExpiration(" + _delegate + ", " + _durationTicks + ", TICKS)";
            }
        }

        public static ISupplier<T> OfInstance<T>([Nullable] T instance)
        {
            return new SupplierOfInstance<T>(instance);
        }

        private class SupplierOfInstance<T> : ISupplier<T>
        {
            private readonly T _instance;

            public SupplierOfInstance([Nullable] T instance)
            {
                _instance = instance;
            }

            public T Get()
            {
                return _instance;
            }

            public override bool Equals(object obj)
            {
                if (obj is SupplierOfInstance<T>)
                {
                    var that = (SupplierOfInstance<T>) obj;
                    return Objects.Equal(_instance, that._instance);
                }

                return false;
            }

            public override int GetHashCode()
            {
                return Objects.HashCode(_instance);
            }

            public override string ToString()
            {
                return "Suppliers.OfInstance(" + _instance + ")";
            }
        }

        public static ISupplier<T> SynchronizedSupplier<T>(ISupplier<T> @delegate)
        {
            return new ThreadSafeSupplier<T>(Preconditions.CheckNotNull(@delegate));
        }

        private class ThreadSafeSupplier<T> : ISupplier<T>
        {
            private readonly ISupplier<T> _delegate;

            public ThreadSafeSupplier(ISupplier<T> @delegate)
            {
                _delegate = @delegate;
            }

            public T Get()
            {
                lock (_delegate)
                {
                    return _delegate.Get();
                }
            }

            public override string ToString()
            {
                return "Suppliers.SynchronizedSupplier(" + _delegate + ")";
            }
        }
    }
}