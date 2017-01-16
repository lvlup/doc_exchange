﻿using System;
using System.ComponentModel;
using System.Reflection;

namespace DocumentsExchange.Common.Utils
{
   public class EnumUtils
    {
        public static string GetStringValue(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}
