using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BusinessLogic.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<KeyValuePair<string, string>> GetAllValuesAndDescriptions<TEnum>() where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum must be an Enumeration type");
            }

            return from enumValues in Enum.GetValues(typeof(TEnum)).Cast<Enum>()
                   select new KeyValuePair<string, string>(enumValues.ToString(), DescriptionAttr(enumValues));
        }

        public static string DescriptionAttr<TEnum>(this TEnum source)
        {
            var fieldInfo = source.GetType().GetField(source.ToString());

            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : source.ToString();
        }
    }
}
