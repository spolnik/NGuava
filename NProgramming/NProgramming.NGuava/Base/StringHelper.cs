using System;

namespace NProgramming.NGuava.Base
{
    public static class StringHelper
    {
        public static string ValueOf(Object obj)
        {
            return (obj == null) ? "null" : obj.ToString();
        }
    }
}