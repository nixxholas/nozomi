using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Nozomi.Base.Core.Helpers.Enumerator
{
    public static class EnumHelper
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }

            return null; // could also return string.Empty
        }
        
        // https://stackoverflow.com/questions/4367723/get-enum-from-description-attribute
        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if(!type.IsEnum) throw new InvalidOperationException();
            foreach(var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if(attribute != null)
                {
                    if(attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if(field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
            // or return default(T);
        }
    }
}