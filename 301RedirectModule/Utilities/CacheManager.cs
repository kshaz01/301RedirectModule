using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;

namespace SharedSource.RedirectModule.Utilities
{
    /// <summary>
    /// Simple cache class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class CacheManager<T> where T : class
    {
        /// <summary>
        /// gets param out of the cache or inserts it if it has expired
        /// </summary>
        /// <param name="key">cache key</param>
        /// <param name="method">method to execute if needed</param>
        /// <param name="expiration">cache timeout</param>
        /// <returns></returns>
        public static T GetValue(string key, Func<T> method, TimeSpan expiration)
        {
            Cache cache = new CacheService().Cache;

            if (cache[key] == null)
            {
                cache.Add(key, method(), null, DateTime.Now + expiration, Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
            }

            return cache[key] as T;
        }

        /// <summary>
        /// gets param out of the cache or inserts it if it has expired
        /// </summary>
        /// <param name="key">cache key</param>
        /// <param name="method">method to execute if needed</param>
        /// <returns></returns>
        public static T GetValue(string key, Func<T> method)
        {
            return GetValue(key, method, new TimeSpan(1, 0, 0));
        }

        /// <summary>
        /// clears the cache
        /// </summary>
        /// <param name="key"></param>
        public static void ClearCache(string key)
        {
            Cache cache = new CacheService().Cache;
            cache.Remove(key);
        }
    }
}
