using System;

namespace NProgramming.NGuava.Utils
{
    public static class ArrayUtils
    {
        /// <summary>
        /// Returns hash code for an array that is generated based on the elements.
        /// </summary>
        /// <remarks>
        /// Hash code returned by this method is guaranteed to be the same for
        /// arrays with equal elements.
        /// </remarks>
        /// <param name="array">
        /// Array to calculate hash code for.
        /// </param>
        /// <returns>
        /// A hash code for the specified array.
        /// </returns>
        public static int GetHashCode(Array array)
        {
            int hashCode = 0;

            if (array != null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    object el = array.GetValue(i);
                    if (el != null)
                    {
                        if (el is Array)
                        {
                            hashCode += 17 * GetHashCode(el as Array);
                        }
                        else
                        {
                            hashCode += 13 * el.GetHashCode();
                        }
                    }
                }
            }

            return hashCode;
        }
    }
}