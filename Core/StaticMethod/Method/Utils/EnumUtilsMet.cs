﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.StaticMethod.Method.Utils {
   public static class EnumUtilsMet {
        /// <summary>
        /// 根据枚举获取对应的valueString值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(Enum value) {
            if (value == null) {
                throw new ArgumentException("value");
            }
            string description = value.ToString();
            var fieldInfo = value.GetType().GetField(description);
            var attributes =
                (EnumDescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                description = attributes[0].Description;
            }
            return description;
        }
        [AttributeUsage(AttributeTargets.Field,AllowMultiple = false)]  
        internal sealed class EnumDescriptionAttribute : Attribute {
            internal string Description { get; }
            internal EnumDescriptionAttribute(string description)  
                : base()  
            {  
                Description = description;  
            }  
        }
   }
}