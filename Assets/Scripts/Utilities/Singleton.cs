using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlsRitter.Utilities
{
    /// <summary>
    /// 单例模板
    /// </summary>
    /// <typeparam name="T">泛型类型得继承自 MonoBehaviour</typeparam>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();

        /// <summary>
        /// 是否需要丢进 DontDestroyOnLoad 里面
        /// </summary>
        public bool isDontDestroyOnLoad = false;

        /// <summary>
        /// 实例的属性
        /// </summary>
        public static T instance
        {
            get
            {
                /*if (_applicationIsQuitting)
                {
                    return null;
                }
*/
                lock (_lock)
                {
                    if (_instance != null) return _instance;

                    // FindObjectOfType 返回找到的第一个对象
                    // 不过这个函数是非常慢的（下同）。不推荐在每帧使用这个函数，大多数情况下你可以使用单例模式代替。
                    _instance = (T) FindObjectOfType(typeof(T));

                    // FindObjectsOfType(typeof(Type)) 返回 Type 类型的所有激活的加载的物体列表（就是返回一个已经实例的对象）
                    // 如果找到多个单例对象则抛出警告
                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        if (Application.isEditor)
                            Debug.LogWarning(
                                "MonoSingleton<T>.Instance: Only 1 singleton instance can exist in the scene. Null will be returned.");

                        return _instance;
                    }

                    if (_instance != null) return _instance;


                    // 如果没有找到则表示不存在这个对象，则需要创建
                    // 自动创建一个名为 “(singleton)类名” 的游戏体，一旦创建，在切换场景的时候该类也不会销毁，所以多个场景只能有一份该类。
                    var singleton = new GameObject(); // 找不到才需要自己创建单例

                    _instance = singleton.AddComponent<T>();
                    singleton.name = "(singleton) " + typeof(T).ToString();

                    return _instance;
                }
            }
        }

        //private static bool _applicationIsQuitting = false;

        public void Awake()
        {
            if (isDontDestroyOnLoad)
            {
                DontDestroyOnLoad(instance);
            }
        }

        /// <summary>
        /// Unity 会在销毁对象时调用这个方法，所以需要将这个对象存到 DontDestroyOnLoad
        /// </summary>
        public void OnDestroy()
        {
            //_applicationIsQuitting = true;
        }
    }
}