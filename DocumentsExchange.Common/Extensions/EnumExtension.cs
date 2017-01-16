using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.Common.Extensions
{
  public static class EnumExtension
    {
        public static ICollection<string> GetValuesFromDescriptionAttribute<TEnum>(this Currency currency)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var result = new List<string>();

            var enumValues = Enum.GetValues(typeof(TEnum));

            foreach (var enumVal in enumValues)
            {
                FieldInfo fi = enumVal.GetType().GetField(enumVal.ToString());
            
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                result.Add(attributes.Length > 0 ? attributes[0].Description : enumVal.ToString());
            }

            return result;
        }
    }
}
