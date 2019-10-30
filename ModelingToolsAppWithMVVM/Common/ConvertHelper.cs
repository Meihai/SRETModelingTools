using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common
{
    /// <summary>
    /// 值类型转换器，可实现枚举和空值类型的转换
    /// </summary>
    public static class ConvertHelper
    {
        #region = ChangeType =
        public static object ChangeType(object obj, Type conversionType)
        {
            object objV=ChangeType(obj, conversionType, Thread.CurrentThread.CurrentCulture);
            return objV;
        }

        public static object ChangeType(object obj, Type conversionType, IFormatProvider provider)
        {
            #region Nullable
            Type nullableType = Nullable.GetUnderlyingType(conversionType);
            if (nullableType != null)
            {
                if (obj == null)
                {
                    return null;
                }
                return Convert.ChangeType(obj, nullableType, provider);
            }
            #endregion Nullable
            if (typeof(System.Enum).IsAssignableFrom(conversionType))
            {
                object enumobj= Enum.Parse(conversionType, obj.ToString());
                return enumobj;
            }
            return Convert.ChangeType(obj, conversionType, provider);

        }
        #endregion
    }
}
