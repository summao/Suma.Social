using System;

namespace Suma.Social.Utils
{
    public static class StringUtil
    {
        public static string ToLowerInvariantFirstLetter(this string value)
        {
            return Char.ToLowerInvariant(value[0]) + value.Substring(1);
        }
    }
}