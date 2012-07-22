using System;
using System.ComponentModel;
using System.Reflection;

namespace Rappers.Baseline.Extensions
{
    public static class EnumExtensions
    {
        public static string Description(this Enum source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute),false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }        
            return source.ToString();
        }
    }
}
