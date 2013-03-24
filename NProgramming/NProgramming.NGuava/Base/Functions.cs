namespace NProgramming.NGuava.Base
{
    public static class Functions
    {
        public static string ToStringFunction(object o)
        {
            Preconditions.CheckNotNull(o);
            return o.ToString();
        }

        [return: Nullable]
        public static T Identity<T>([Nullable] T o)
        {
            return o;
        }
    }
}