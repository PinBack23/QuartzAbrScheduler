using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Text;

namespace Abraxas.Scheduler.Core
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNotEmpty(this string value)
        {
            return !value.IsEmpty();
        }

        public static string EmptyOrValue(this string value)
        {
            return value.IfEmpty(string.Empty);
        }

        public static string IfEmpty(this string value, string defaultValue)
        {
            return (!value.IsEmpty() ? value : defaultValue);
        }

        public static string FormatWith(this string value, params object[] parameters)
        {
            return string.Format(value, parameters);
        }

        public static string UpperTrim(this string value)
        {
            return (value.IsEmpty() ? string.Empty : value.Trim().ToUpper());
        }
    }
}
