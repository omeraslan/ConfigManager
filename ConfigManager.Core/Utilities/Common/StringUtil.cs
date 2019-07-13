using System;
using System.ComponentModel;

namespace ConfigManager.Core.Utilities.Common
{
    public static class StringUtil
    {
        /// <summary>
        /// 	Trims the text to a provided maximum length.
        /// </summary>
        /// <param name = "value">The input string.</param>
        /// <param name = "maxLength">Maximum length.</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public static string TrimToMaxLength(this string value, int maxLength)
        {
            return (value == null || value.Length <= maxLength ? value : value.Substring(0, maxLength));
        }

        /// <summary>
        /// String'den istenilen tipe cast eder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T Cast<T>(this string input, string type)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(GetType(type));
                return (T)converter.ConvertFromString(input);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        private static Type GetType(string type)
        {
            switch (type)
            {
                case "String":
                    return typeof(string);
                case "Boolean":
                    return typeof(bool);
                case "Int":
                    return typeof(int);
                case "Double":
                    return typeof(double);
                default:
                    return typeof(string);
            }
        }
    }
}
