using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.SessionState;
using System.Web.Caching; 

namespace PandaClass
{
    /// <summary>
    /// 缓存操作相关的公共类
    /// </summary>
    public sealed class CacheHelper
    {
        #region 将目标对象存储到缓存中

        #region 重载1
        /// <summary>
        /// 将目标对象存储到缓存中
        /// </summary>
        /// <typeparam name="T">目标对象的类型</typeparam>
        /// <param name="key">缓存项的键名</param>
        /// <param name="target">目标对象</param>
        public static void SetCache<T>(string key, T target)
        {
            try
            {
                //将目标对象存储到缓存中
                HttpRuntime.Cache.Insert(key, target);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 重载2
        /// <summary>
        /// 将目标对象存储到缓存中
        /// </summary>
        /// <typeparam name="T">目标对象的类型</typeparam>
        /// <param name="key">缓存项的键名</param>
        /// <param name="target">目标对象</param>
        /// <param name="dependencyFilePath">依赖的文件绝对路径,当该文件更改时,则将该项移出缓存</param>
        public static void SetCache<T>(string key, T target, string dependencyFilePath)
        {
            try
            {
                //创建缓存依赖
                CacheDependency dependency = new CacheDependency(dependencyFilePath);

                //将目标对象存储到缓存中
                HttpRuntime.Cache.Insert(key, target, dependency);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 重载3
        /// <summary>
        /// 写入缓存【设置小时】
        /// </summary>
        public static void SetCacheHours<T>(string key, T target, int Hour)
        {
            HttpRuntime.Cache.Insert(key, target, null, DateTime.Now.AddHours(Hour), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
        }
        /// <summary>
        /// 写入缓存【设置分钟】
        /// </summary>
        public static void SetCacheMinutes<T>(string key, T target, int Minutes)
        {
            HttpRuntime.Cache.Insert(key, target, null, DateTime.Now.AddMinutes(Minutes), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
        }
        /// <summary>
        /// 写入缓存【设置天数】
        /// </summary>
        public static void SetCacheDays<T>(string key, T target, int Days)
        {
            HttpRuntime.Cache.Insert(key, target, null, DateTime.Now.AddDays(Days), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
        }
        /// <summary>
        /// 写入缓存【数据库依赖】
        /// </summary>
        public static void SetCacheDependency<T>(string key, T target, SqlCacheDependency dependency)
        {
            HttpRuntime.Cache.Insert(key, target, dependency, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
        }
        /// <summary>
        /// 写入缓存【设置月数】
        /// </summary>
        public static void SetCacheMonths<T>(string key, T target, int Months)
        {
            HttpRuntime.Cache.Insert(key, target, null, DateTime.Now.AddMonths(Months), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
        }
        /// <summary>
        /// 写入缓存【设置年数】
        /// </summary>
        public static void SetCacheYears<T>(string key, T target, int Years)
        {
            HttpRuntime.Cache.Insert(key, target, null, DateTime.Now.AddYears(Years), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
        }
        /// <summary>
        /// 写入缓存【自上次访问后 ? 分钟过期】
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="target"></param>
        /// <param name="minute"></param>
        public static void SaveCacheMinuteSliding<T>(string key, T target, int minute)
        {
            HttpRuntime.Cache.Insert(key, target, null, DateTime.MaxValue, TimeSpan.FromMinutes(minute));
        }
        /// <summary>
        /// 写入缓存【不过期】
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        public static void SavaCacheNoOverdue<T>(string key, T target)
        {
            HttpRuntime.Cache.Insert(key, target, null);
        }
        #endregion

        #endregion

        #region 获取缓存中的目标对象
       
        /// <summary>
        /// 获取缓存中的目标对象
        /// </summary>
        /// <typeparam name="T">目标对象的类型</typeparam>
        /// <param name="key">缓存项的键名</param>
        public static object GetCache(string key)
        {
            //获取缓存中的目标对象
            return (HttpRuntime.Cache.Get(key));
        }
        #endregion

        #region 删除指定缓存
        /// <summary>
        /// 清空缓存
        /// </summary>
        /// <param name="key"></param>
        public static void ClearCache(string key)
        {
            if (null != HttpRuntime.Cache[key])
                HttpRuntime.Cache.Remove(key);
        }
        #endregion

        #region 检测目标对象是否存储在缓存中
        /// <summary>
        /// 检测目标对象是否存储在缓存中,存在返回true
        /// </summary>
        /// <param name="key">缓存项的键名</param>
        public static bool Contains(string key)
        {
            try
            {
                //将目标对象存储到缓存中
                return ValidationHelper.IsNullOrEmpty(HttpRuntime.Cache.Get(key)) ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
