using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PowerUpSystem
{
    /// <summary>
    /// 消息管理
    /// </summary>
    public class EventSystemListeners : Singleton<EventSystemListeners>
    {
        [Tooltip("默认监听带有 Listener 标签的对象，不过这也不是必须的，可以手动调用 AddListener 方法将对象添加进去")]
        public List<GameObject> listeners;


        // Use this for initialization
        private void Start()
        {
            // Look for every object tagged as a listener
            if (listeners == null)
            {
                listeners = new List<GameObject>();
            }

            var gos = GameObject.FindGameObjectsWithTag("Listener");
            listeners.AddRange(gos);
        }

        public void AddListener(GameObject go)
        {
            // Don't add if already there
            if (!listeners.Contains(go))
            {
                listeners.Add(go);
            }
        }

    }
}