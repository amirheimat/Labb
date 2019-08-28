using Supermarket.API.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Supermarket.API.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescriptionString<TEnum>(this TEnum @enum)
        {
            FieldInfo info = @enum.GetType().GetField(@enum.ToString());
            var attributes = (DescriptionAttribute[])info.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes?[0].Description ?? @enum.ToString();
        }

        public static bool IsWeight(this EUnitOfMeasurement @enum)
        {
            // Show switch expression!
            return @enum switch
            {
                EUnitOfMeasurement.Gram => true,
                EUnitOfMeasurement.Kilogram => true,
                EUnitOfMeasurement.Milligram => true,
                _ => false,
            };
        }
    }
}
