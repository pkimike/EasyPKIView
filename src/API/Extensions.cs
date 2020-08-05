using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPKIView
{
    internal static class Extensions
    {
        internal static bool Matches(this string expression, string compare, bool caseSensitive = false, bool ignoreWhiteSpace = true)
        {
            if (ignoreWhiteSpace)
            {
                expression = expression.Trim();
                compare = compare.Trim();
            }
            StringComparison comparisonType = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            return string.Equals(expression, compare, comparisonType);
        }

        internal static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = Enum.GetValues(type);
                foreach(int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        DescriptionAttribute DescAttribute = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false)
                                                                       .FirstOrDefault() as DescriptionAttribute;

                        if (DescAttribute != null)
                        {
                            return DescAttribute.Description;
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                }
            }
            return string.Empty;
        }

        internal static TimeSpan ToTimeSpan(this byte[] value)
        {
            long period = BitConverter.ToInt64(value, 0);
            period /= -10000000; 
            return TimeSpan.FromSeconds(period);
        }

        internal static byte[] ToByteArray(this TimeSpan value)
        {
            double period = value.TotalSeconds;
            period *= -10000000;
            return BitConverter.GetBytes((long)period);
        }
    }
}
