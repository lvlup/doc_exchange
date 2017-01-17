using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsExchange.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        public static int ToInt(this string source, int def = default(int))
        {
            int res;
            return int.TryParse(source, out res) ? res : def;
        }
    }
}
