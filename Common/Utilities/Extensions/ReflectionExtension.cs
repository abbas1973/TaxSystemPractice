﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Utilities
{
    public static class ReflectionExtension
    {

        #region آیا یک کلاس مشخص، پروپرتی با نام مشخص دارد؟
        /// <summary>
        /// آیا یک کلاس مشخص، پروپرتی با نام مشخص دارد؟
        /// typeof(MyClass).HasProperty("propname");
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool HasProperty(this Type obj, string propertyName)
        {
            return obj.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) != null;
        }
        #endregion



        #region گرفتن اتریبیوت دسکریپشن از FieldInfo
        /// <summary>
        /// گرفتن اتریبیوت دسکریپشن از FieldInfo
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static string GetDescription(this FieldInfo field)
        {
            if (field != null)
            {
                DescriptionAttribute attr =
                       Attribute.GetCustomAttribute(field,
                         typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attr != null)
                {
                    return attr.Description;
                }
            }
            return null;
        }
        #endregion


        #region گرفتن اتریبیوت دسکریپشن از Type
        /// <summary>
        /// گرفتن اتریبیوت دسکریپشن از Type
        /// </summary>
        /// <returns></returns>
        public static string GetDescription(this Type type)
        {
            if (type != null)
            {
                var descriptions = (DescriptionAttribute[])type.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (descriptions.Length == 0)
                    return null;
                return descriptions[0].Description;
            }
            return null;
        }
        #endregion



        #region گرفتن اطلاعات فیلدهای از نوع const در یک کلاس
        public static List<FieldInfo> GetConstants(
            this Type type,
            params string[] propNames)
        {
            #region گرفتن فیلد ها
            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly);

            //IEnumerable<FieldInfo> fieldInfos = type.GetFields(flags);
            //fieldInfos = fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly);
            if (propNames != null && propNames.Any())
                fieldInfos = fieldInfos.Where(x => propNames.Contains(x.Name));
            #endregion

            return fieldInfos.ToList();
        }
        #endregion



        #region گرفتن مقدار فیلدهای از نوع const در یک کلاس
        public static List<string> GetConstantValues(
            this Type type,
            params string[] propNames)
        {
            var consts = type.GetConstants(propNames);

            return consts.Select(x => x.GetValue(null).ToString()).ToList();
        }
        #endregion



        #region گرفتن مقدار و توضیحات const های پابلیک یک کلاس
        public static List<KeyValuePair<string, string>> GetConstantsWithDescription
            (this Type type,
            params string[] propNames)
        {
            var model = new List<KeyValuePair<string, string>>();
            var consts = type.GetConstants(propNames);
            foreach (var item in consts)
            {
                var val = item.GetValue(null).ToString();
                model.Add(new KeyValuePair<string, string>(val, item.GetDescription()));
            }
            return model;
        }
        #endregion




        #region اضافه کردن پراپرتی به آبجک داینامیک سی شارپ
        /// <summary>
        /// اضافه کردن پراپرتی به آبجک داینامیک سی شارپ
        /// </summary>
        /// <param name="expando">آبجکت</param>
        /// <param name="propertyName">نام پراپرتی</param>
        /// <param name="propertyValue">مقدار مورد نظر</param>
        public static void AddProperty(this ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
        #endregion



        #region تبدیل بین انواع داده بصورت رفلکشن و برای تایپ های نال پذیر
        /// <summary>
        /// تبدیل بین انواع داده بصورت رفلکشن و برای تایپ های نال پذیر
        /// <para>
        /// string.ChangeType("123", typeof(int))
        /// </para>
        /// </summary>
        /// <param name="value">مقدار</param>
        /// <param name="conversion">نوع مقصد</param>
        /// <returns></returns>
        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            // اگر نوع عددی بود ممکنه بین عددها ویرگول داشته باشه که باید حذف بشه
            if (t == typeof(int) || t == typeof(long) || t == typeof(uint))
                value = value?.ToString().Trim().Replace(",", "");
            return Convert.ChangeType(value, t);
        }
        #endregion
    }
}

