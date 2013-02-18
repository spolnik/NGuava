using System;

namespace NProgramming.NGuava.Base
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public sealed class NullableAttribute : Attribute
    {
    }
}