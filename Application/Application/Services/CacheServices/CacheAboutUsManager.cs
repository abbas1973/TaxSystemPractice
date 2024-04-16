//using Domain.Entities;
//using DTO.Category;
//using Microsoft.Extensions.Caching.Memory;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Application.CacheServices
//{
//    /// <summary>
//    /// مدیریت درباره ما درون کش
//    /// </summary>
//    public static class CacheAboutUsManager
//    {
//        public static readonly string Key = "AboutUs";



//        public static AboutUs GetAboutUs(this IMemoryCache cache)
//        {
//            return cache.Get<AboutUs>(Key);
//        }


//        public static void setAboutUs(this IMemoryCache cache, AboutUs value)
//        {
//            cache.Set<AboutUs>(Key, value);
//        }



//        public static void RemoveAboutUs(this IMemoryCache cache)
//        {
//            cache.Remove(Key);
//        }

//    }
//}
