using System;
using System.Collections.Generic;
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
    }
}
