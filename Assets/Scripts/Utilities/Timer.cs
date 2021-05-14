using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace AlsRitter.Utilities
{
    /// <summary>
    /// 计时器
    /// 参考资料：Unity计时器脚本Timer：https://segmentfault.com/a/1190000015325310
    /// </summary>
    public class Timer : MonoBehaviour
    {
        // 延迟时间(秒)
        public float delay = 0;
        // 间隔时间(秒)
        public float interval = 1;
        // 重复次数(为 -1 表示无限重复)
        public int repeatCount = 1;
        // 自动计时
        public bool autoStart = false;
        // 自动销毁
        public bool autoDestory = true;
        // 当前时间
        public float currentTime = 0;
        // 当前次数
        public int currentCount = 0;
        // 计时间隔（这里可以通过 Unity 提供的事件系统绑定任意脚本的公共方法）
        // 与下面的 onIntervalCall 本质是一样的，只不过下面那个是用户自己传入的回调函数
        public UnityEvent onIntervalEvent;
        // 计时完成
        public UnityEvent onCompleteEvent;

        // 回调事件代理
        public delegate void TimerCallback(Timer timer);
        // 上一次间隔时间
        private float lastTime = 0;

        // 计时间隔
        private TimerCallback onIntervalCall;
        // 计时结束
        private TimerCallback onCompleteCall;

        private void Start()
        {
            enabled = autoStart;
        }

        private void FixedUpdate()
        {
            if (!enabled) return;
            addInterval(Time.deltaTime);
        }

        /// <summary> 增加间隔时间 </summary>
        private void addInterval(float deltaTime)
        {
            currentTime += deltaTime;
            if (currentTime < delay) return;
            if (currentTime - lastTime >= interval)
            {
                currentCount++;
                lastTime = currentTime;
                if (repeatCount <= 0)
                {
                    // 无限重复
                    if (currentCount == int.MaxValue) reset();

                    // 每计数间隔都执行一次的函数
                    // 这里两个方法都通知调用（这两种只是绑定方法的方式不一样而已）
                    // 
                    // 第一个是用户通过 start 方法传入的回调函数，
                    // 第二个是通过在编辑器界面指定调用的方法）
                    onIntervalCall?.Invoke(this);
                    onIntervalEvent?.Invoke(); 
                }
                else
                {
                    if (currentCount < repeatCount)
                    {
                        //计时间隔
                        onIntervalCall?.Invoke(this);
                        onIntervalEvent?.Invoke();
                    }
                    else
                    {
                        //计时结束
                        stop();
                        onCompleteCall?.Invoke(this);
                        onCompleteEvent?.Invoke();
                        if (autoDestory && !enabled) Destroy(this);
                    }
                }
            }
        }

        /// <summary> 开始/继续计时 </summary>
        public void start()
        {
            enabled = autoStart = true;
        }

        /// <summary> 开始计时 </summary>
        /// <param name="time">时间(秒)</param>
        /// <param name="onComplete(Timer timer)">计时完成回调事件</param>
        public void start(float time, TimerCallback onComplete)
        {
            start(time, 1, null, onComplete);
        }

        /// <summary> 开始计时 </summary>
        /// <param name="interval">计时间隔</param>
        /// <param name="repeatCount">重复次数</param>
        /// <param name="onComplete(Timer timer)">计时完成回调事件</param>
        public void start(float interval, int repeatCount, TimerCallback onComplete)
        {
            start(interval, repeatCount, null, onComplete);
        }

        /// <summary> 开始计时 </summary>
        /// <param name="interval">计时间隔</param>
        /// <param name="repeatCount">重复次数</param>
        /// <param name="onInterval(Timer timer)">计时间隔回调事件</param>
        /// <param name="onComplete(Timer timer)">计时完成回调事件</param>
        public void start(float interval, int repeatCount, TimerCallback onInterval, TimerCallback onComplete)
        {
            this.interval = interval;
            this.repeatCount = repeatCount;
            onIntervalCall = onInterval;
            onCompleteCall = onComplete;

            reset();
            enabled = autoStart = true;
        }

        /// <summary> 暂停计时 </summary>
        public void stop()
        {
            enabled = autoStart = false;
        }


        /// <summary> 停止Timer并重置数据 </summary>
        public void reset()
        {
            lastTime = currentTime = currentCount = 0;
        }

        /// <summary> 重置数据并重新开始计时 </summary>
        public void restart()
        {
            reset();
            start();
        }
    }
}