using NProgramming.NGuava.Base;

namespace NProgramming.NGuava.Utils
{
    public static class Objects
    {
         public static int HashCode([Nullable] params object[] objects)
         {
             return ArrayUtils.GetHashCode(objects);
         }

         public static bool Equal([Nullable] object a, [Nullable] object b)
         {
             return a == b || (a != null && a.Equals(b));
         }
    }
}