using System;
using System.ComponentModel;

namespace EMS.Domain
{
    public enum DatePartEnum
    {
        Am,
        Pm,
        [Description("Full Day")]
        FullDay
    }

    public static class Extensions
    {
        public static string Description(this DatePartEnum @enum)
        {
            var description = string.Empty;
            var fields = @enum.GetType().GetFields();
            foreach (var field in fields)
            {
                var descriptionAttribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (descriptionAttribute != null &&
                    field.Name.Equals(@enum.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    description = descriptionAttribute.Description;
                    break;
                }
            }

            return description;
        }
    }
}