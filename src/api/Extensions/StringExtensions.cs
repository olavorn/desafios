using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace api.Extensions
{
    public static class StringExtensions
    {
        public static string OnlyNumbers(this string str)
        {
            if (str == null) return str;
            return Regex.Replace(str, "[^\\d]+", string.Empty);
        }

        public static string RemoveUrlProtocol(this string str)
        {
            return Regex.Replace(str, @"https?:\/\/(www\.)?", string.Empty);
        }
    }
}
