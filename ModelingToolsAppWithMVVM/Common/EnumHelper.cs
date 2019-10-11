using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ModelingToolsAppWithMVVM.Common
{
    public static class EnumHelper
    {

        public static string GetDescription(this object value)
        {
            if (value == null)
                return string.Empty;

            Type type = value.GetType();
            var fieldInfo = type.GetField(Enum.GetName(type, value));
            if (fieldInfo != null)
            {
                if (Attribute.IsDefined(fieldInfo, typeof(DescriptionAttribute)))
                {
                    var description =
                        Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;

                    if (description != null)
                        return description.Description;
                }
            }
            return string.Empty;
        }
    }
}
