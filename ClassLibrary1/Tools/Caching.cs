using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace DAL.Tools
{
    public class Caching
    {
        public static object Get(string name)
        {
            return HttpRuntime.Cache.Get(name);
        }

        public static void Remove(string name)
        {
            if (HttpRuntime.Cache[name] != null)
                HttpRuntime.Cache.Remove(name);
        }
        public static void RemoveStartWith(string cacheStart)
        {
            List<string> cacheKeys = new List<string>();
            IDictionaryEnumerator cacheEnum = HttpContext.Current.Cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cacheKeys.Add(cacheEnum.Key.ToString());
            }
            foreach (string cacheKey in cacheKeys)
            {
                if (cacheKey.StartsWith(cacheStart))
                    HttpContext.Current.Cache.Remove(cacheKey);
            }
        }

        public static void RemoveAllCache()
        {
            List<string> cacheKeys = new List<string>();
            IDictionaryEnumerator cacheEnum = HttpContext.Current.Cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cacheKeys.Add(cacheEnum.Key.ToString());
            }
            foreach (string cacheKey in cacheKeys)
            {
                if (cacheKey.StartsWith("GetSubNodeAll"))
                    HttpContext.Current.Cache.Remove(cacheKey);
            }
        }

        public static void Set(string name, object value)
        {
            Set(name, value, null);
        }

        public static void Set(string name, object value, CacheDependency cacheDependency)
        {
            HttpRuntime.Cache.Insert(name, value, cacheDependency, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20));
        }

        public static void Set(string name, object value, int minutes)
        {
            HttpRuntime.Cache.Insert(name, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(minutes));
        }
    }
}
