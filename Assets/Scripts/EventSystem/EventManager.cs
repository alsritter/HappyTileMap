using System;
using System.Collections.Generic;
using UnityEngine;

namespace EventFrame
{
    /// <summary>
    /// 事件的管理器
    /// </summary>
    public class EventManager : Singleton<EventManager>
    {
        /// <summary>
        /// 维护一个观察者队列
        /// </summary>
        private readonly Dictionary<EventID, List<IEventObserver>> observerList =
            new Dictionary<EventID, List<IEventObserver>>();

        private readonly Queue<EventData> eventQueue = new Queue<EventData>(); //消息队列


        private void Update()
        {
            while (eventQueue.Count > 0)
            {
                // 从队列弹出事件
                EventData eve = eventQueue.Dequeue();

                // 如果没有观察者监听这个事件则继续下一个事件的分发
                if (!observerList.ContainsKey(eve.eid)) continue;

                // 通知监听了这个事件的全部观察者
                List<IEventObserver> observers = observerList[eve.eid];
                for (int i = 0; i < observers.Count; i++)
                {
                    if (observers[i] == null) continue;
                    observers[i].HandleEvent(eve);
                }
            }
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="eve"></param>
        internal void SendEvent(EventData eve)
        {
            eventQueue.Enqueue(eve);
        }

        /// <summary>
        /// 注册一个监听者
        /// </summary>
        /// <param name="newobj">需要注册的监听者</param>
        /// <param name="eid">需要监听的事件 ID</param>
        private void RegisterObj(IEventObserver newobj, EventID eid)
        {
            if (!observerList.ContainsKey(eid))
            {
                var list = new List<IEventObserver> {newobj};
                observerList.Add(eid, list);
            }
            else
            {
                List<IEventObserver> list = observerList[eid];
                foreach (IEventObserver obj in list)
                {
                    if (obj == newobj)
                    {
                        return;
                    }
                }

                list.Add(newobj);
            }
        }

        /// <summary>
        /// 移除监听者
        /// </summary>
        /// <param name="removeObj">需要移除的监听对象</param>
        private void RemoveObj(IEventObserver removeObj)
        {
            foreach (KeyValuePair<EventID, List<IEventObserver>> kv in observerList)
            {
                List<IEventObserver> list = kv.Value;
                foreach (IEventObserver obj in list)
                {
                    if (obj == removeObj)
                    {
                        list.Remove(obj);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 移除一个监听者
        /// </summary>
        /// <returns>The remove.</returns>
        /// <param name="removeObj">需要移除的对象</param>
        public static void Remove(IEventObserver removeObj)
        {
            if (EventManager.instance == null) return;
            EventManager.instance.RemoveObj(removeObj);
        }

        /// <summary>
        /// 监听者在这里注册
        ///
        /// 注意这里形参的 params 就是 Java 的可变参数
        /// </summary>
        /// <param name="newobj">需要被注册的监听者</param>
        /// <param name="eids">需要监听的事件列表.</param>
        public static void Register(IEventObserver newobj, params EventID[] eids)
        {
            if (EventManager.instance == null) return;
            foreach (EventID eid in eids)
            {
                EventManager.instance.RegisterObj(newobj, eid);
            }
        }
    }
}
