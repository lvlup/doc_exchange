using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace DocumentsExchange.Common.Extensions
{
    public static class EnumExtension
    {
        public static string GetValueFromDescriptionAttribute<TEnum>(this TEnum currency)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            string result = currency.ToString();

            FieldInfo fi = typeof(TEnum).GetField(currency.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
                result = attributes[0].Description;

            return result;
        }

        public static string[] GetEnumDescriptions<TEnum>()
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            return Enum.GetValues(typeof(TEnum)).OfType<TEnum>().Select(x => x.GetValueFromDescriptionAttribute()).ToArray();
        }
    }
}
