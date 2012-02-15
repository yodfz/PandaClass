using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace PandaClass
{
    /// <summary>
    /// 测试工具
    /// </summary>
    public class Test
    {
        #region 字段定义
        /// <summary>
        /// 测试运行时间
        /// </summary>
        private Stopwatch _watch;
        #endregion

        #region 开始计时
        /// <summary>
        /// 开启计时器,开始计算运行时间
        /// </summary>
        public void StartWatch()
        {
            //创建Stopwatch实例
            _watch = new Stopwatch();

            //开始计时
            _watch.Start();
        }
        #endregion

        #region 重置计时器
        /// <summary>
        /// 重置计时器
        /// </summary>
        public void ResetWatch()
        {
            //重置计时器
            _watch.Reset();
        }
        #endregion

        #region 获取运行的时间间隔
        /// <summary>
        /// 获取运行的时间间隔
        /// </summary>
        public TimeSpan GetElapsed()
        {
            //返回时间间隔
            return _watch.Elapsed;
        }
        #endregion

        #region 获取运行的时间间隔,同时停止计时
        /// <summary>
        /// 获取运行的时间间隔,同时停止计时
        /// </summary>
        public TimeSpan GetElapsedAndStop()
        {
            //停止计时
            _watch.Stop();

            //返回时间间隔
            return _watch.Elapsed;
        }
        #endregion

        #region 获取运行的时间间隔,同时停止计时
        /// <summary>
        /// 获取运行的时间间隔,同时停止计时( 单位：毫秒 )
        /// </summary>
        public long GetElapsedMillisecondsAndStop()
        {
            //停止计时
            _watch.Stop();

            //返回时间间隔
            return _watch.ElapsedMilliseconds;
        }
        #endregion

        #region 获取运行的时间间隔,同时停止计时
        /// <summary>
        /// 获取运行的时间间隔,同时停止计时( 用计数器刻度表示 )
        /// </summary>
        public long GetElapsedTicksAndStop()
        {
            //停止计时
            _watch.Stop();

            //返回时间间隔
            return _watch.ElapsedTicks;
        }
        #endregion

        #region 停止计时
        /// <summary>
        /// 停止计时
        /// </summary>
        public void StopWatch()
        {
            _watch.Stop();
        }
        #endregion
    }
}
