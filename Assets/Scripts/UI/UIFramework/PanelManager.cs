using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AlsRitter.Utilities;
using UnityEngine;


namespace AlsRitter.UIFrame
{
    /// <summary>
    /// 面板管理器
    /// </summary>
    public class PanelManager : Singleton<PanelManager>
    {
        //字典存储所有面板的 Prefabs 路径
        private Dictionary<UIPanelType, string> panelPathDict;

        //保存所有已实例化面板的游戏物体身上的BasePanel组件
        private readonly Dictionary<UIPanelType, BasePanel> panelDict;

        //存储当前场景中的界面
        private readonly Stack<BasePanel> panelStack;

        // 页面的画布
        private Transform canvasTransform;

        /// <summary>
        /// 注意，这里的 get 不能直接写在上面的 canvasTransform
        /// </summary>
        private Transform CanvasTransform
        {
            // 因为场景里的画布可能会随着场景销毁而销毁，但是 PanelManager 并不会销毁，
            // 所以需要通过这个机制保证每次都能取得场景的画布
            get
            {
                if (canvasTransform == null)
                {
                    canvasTransform = GameObject.Find("Canvas").transform;
                }

                return canvasTransform;
            }
        }

        public PanelManager()
        {
            panelPathDict = new Dictionary<UIPanelType, string>();
            panelDict = new Dictionary<UIPanelType, BasePanel>();
            panelStack = new Stack<BasePanel>();
        }

        public override void AwakeInitInfo()
        {
            // 解析JSON，获取所有面板的路径信息
            LoadJsonTool.ParseUiPanelTypeJsonData(ref panelPathDict);

/*            // 把场景里面的已经存在的 UI 实例塞进字典
            var panels = FindObjectsOfType<BasePanel>().ToList();
            panels.Sort((x, y) => x.transform.GetSiblingIndex() - y.transform.GetSiblingIndex()); // 升序
            panels.ForEach(x =>
            {
                panelDict.Add(x.uiType, x);
                // 入栈
                panelStack.Push(x);
                //Debug.Log($"当前入栈的是：{x.uiType} 它的索引为：{x.transform.GetSiblingIndex()}");
            });*/
        }

/*        public override void StartInitInfo()
        {
            Debug.Log($"初始化当前对象 {GetHashCode()}");
            // 先清空数据
            panelDict.Clear();
            panelStack.Clear();

            // 把场景里面的已经存在的 UI 实例塞进字典
            var panels = FindObjectsOfType<BasePanel>().ToList();
            panels.Sort((x, y) => x.transform.GetSiblingIndex() - y.transform.GetSiblingIndex()); // 升序
            panels.ForEach(x =>
            {
                panelDict.Add(x.uiType, x);
                // 入栈
                panelStack.Push(x);
                //Debug.Log($"当前入栈的是：{x.uiType} 它的索引为：{x.transform.GetSiblingIndex()}");
            });
        }*/


        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="panel"></param>
        public void Register(BasePanel panel)
        {
            if (!panelDict.ContainsKey(panel.uiType))
            {
                panelDict.Add(panel.uiType, panel);
            }
        }

        /// <summary>
        /// 根据面板类型，返回对应的BasePanel组件
        /// </summary>
        /// <param name="panelType">需要返回的面板类型</param>
        /// <returns>返回该面板组件</returns>
        private BasePanel GetPanel(UIPanelType panelType)
        {
            //如果panel为空，根据该面板 prefab 的路径，实例化该面板
            if (!panelDict.TryGetValue(panelType, out var basePanel))
            {
                var path = panelPathDict[panelType];
                var newPanel = Instantiate(Resources.Load<GameObject>(path)) as GameObject;
                newPanel.transform.SetParent(CanvasTransform, false);

                //第一次实例化的面板需要保存在字典中
                panelDict.Add(panelType, newPanel.GetComponent<BasePanel>());
                return newPanel.GetComponent<BasePanel>();
            }
            else
            {
                return basePanel;
            }
        }

        /// <summary>
        /// 设置默认的栈顶元素
        /// </summary>
        /// <param name="panelType">界面类型</param>
        /// <param name="basePanel">组件</param>
        public void SetDefaultPopPanel(UIPanelType panelType, BasePanel basePanel)
        {
            panelDict.Add(panelType, basePanel);
            panelStack.Push(basePanel);
        }

        /// <summary>
        /// 把该页面显示在场景中
        /// </summary>
        /// <param name="panelType">需要显示界面的类型</param>
        public void PushPanel(UIPanelType panelType)
        {
            //判断一下栈里面是否有页面
            if (panelStack.Count > 0)
            {
                panelStack.Peek().IsPause = true; //原栈顶界面暂停
            }

            var panel = GetPanel(panelType);
            panel.OnEnter(); //调用进入动作
            panelStack.Push(panel); //页面入栈
        }

        /// <summary>
        /// 关闭栈顶界面显示
        /// </summary>
        public void PopPanel()
        {
            //当前栈内为空，则直接返回
            if (panelStack.Count <= 0) return;
            panelStack.Pop().OnExit(); //Pop 删除栈顶元素，并关闭栈顶界面的显示，
            if (panelStack.Count <= 0) return; // 避免删除栈顶原始后就没有了，所以需要再加一个，健壮性判断
            panelStack.Peek().IsPause = false; //获取现在栈顶界面，并调用界面恢复动作(Peek 返回栈顶原始且不删除)
        }

        /// <summary>
        /// 移除 UIPanelType 里面的
        /// </summary>
        /// <param name="panel"></param>
        public void Remove(BasePanel panel)
        {
            // 创建一个临时的栈
            var tempStack = new Stack<BasePanel>();
            //Debug.Log($"销毁了 {panel}");
            while (panelStack.Count != 0)
            {
                var pop = panelStack.Pop();
                if (pop != panel)
                {
                    tempStack.Push(pop);
                }
            }
            // 填回去
            while (tempStack.Count != 0)
            {
                panelStack.Push(tempStack.Pop());
            }

            panelDict.Remove(panel.uiType);
        }
    }
}